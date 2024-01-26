using UnityEngine;
using UnityEngine.Assertions;

namespace Atelier.Physics {

    /// <summary>
    /// Kind of the crown jewel of this code.
    /// 
    /// Supposed to be a drop-in(-ish) replacement for the 3d CharacterController component.
    /// 
    /// Shamelessly studied some Godot source code to figure out how to do this.
    /// 
    /// The MoveAndSlide method is the star of this class, since it implements exactly the behavior
    /// that kinematic Rigidbody2D's don't that I wish they did.
    /// </summary>
    [AddComponentMenu("Atelier/Physics/Kinematic Body 2D")]
    [RequireComponent(typeof(Rigidbody2D))]
    public class KinematicBody2D : MonoBehaviour {

        private const float Tiny = 0.0001f;

        public struct Collision {
            /// <summary>
            /// Point which rigid body moved to due to collision.
            /// </summary>
            public Vector2 point;

            /// <summary>
            /// The normal of the surface the body collided with.
            /// </summary>
            public Vector2 normal;

            /// <summary>
            /// The motion of the body up to the collision.
            /// </summary>
            public Vector2 motion;

            /// <summary>
            /// The motion which the body was not able to complete, due to the collision.
            /// </summary>
            public Vector2 remainder;
        }

        //
        // Parameters
        //

        [SerializeField] private int hitBufferSize = 1;
        [SerializeField] private float margin = 0.08f;
        [SerializeField] private bool stopOnSlope = true;
        [SerializeField] private int maxSlides = 4;
        [SerializeField] private float maxSlopeAngle = 45.0f;
        [SerializeField] private Vector2 gravity = Vector2.down;
        [SerializeField] private LayerMask collisionLayer;

        //
        // Components
        //

        private Rigidbody2D body;

        //
        // Properties
        //

        public float Margin => this.margin;
        public bool IsOnFloor { get; private set; }
        public bool IsOnWall { get; private set; }
        public bool IsOnCeiling { get; private set; }
        public Vector2 FloorNormal { get; private set; }
        public Vector2 Position => this.body.position;
        public float Rotation => this.body.rotation;
        public Vector2 Velocity {
            get => this.velocity;
            set => this.velocity = value;
        }
        public Rigidbody2D Rigidbody => this.body;
        public RaycastHit2D[] HitResults => this.hitBuffer;

        //
        // Internal Data
        //

        private ContactFilter2D contactFilter;
        private RaycastHit2D[] hitBuffer;
        private float minSlopeDotProduct;
        private Vector2 velocity = Vector2.zero;


        //
        // Unity API
        //

        private void Awake() {
            this.body = GetComponent<Rigidbody2D>();
            Assert.IsTrue(body.isKinematic);

            this.contactFilter.useTriggers = false;
            this.contactFilter.SetLayerMask(this.collisionLayer);
            this.contactFilter.useLayerMask = true;

            Assert.IsTrue(hitBufferSize > 0);
            this.hitBuffer = new RaycastHit2D[hitBufferSize];

            this.minSlopeDotProduct = Mathf.Cos(this.maxSlopeAngle * Mathf.Deg2Rad);
        }

        //
        // Component API
        //

        public bool TestMoveAndCollide(Vector2 movement) {
            return this.TestMoveAndCollide(movement, out _);
        }

        public bool TestMoveAndCollide(Vector2 movement, out RaycastHit2D collision) {
            return this.TestMoveAndCollide(movement, this.collisionLayer, out collision);
        }

        public bool TestMoveAndCollide(Vector2 movement, LayerMask mask) {
            return this.TestMoveAndCollide(movement, mask, out _);
        }

        public bool TestMoveAndCollide(Vector2 movement, LayerMask mask, out RaycastHit2D collision) {
            RaycastHit2D[] hitBuffer = this.hitBuffer;
            Vector2 direction = movement.normalized;
            float distance = movement.magnitude + this.margin;

            int hits = this.Cast(direction, mask, hitBuffer, distance);

            if (hits > 0) {
                collision = hitBuffer[0];
                return true;
            } else {
                collision = default;
                return false;
            }
        }

        public float RotateBy(float rotation) {
            float previous = this.body.rotation;
            float next = previous + rotation;
            return this.RotateTo(previous, next);
        }

        // TODO: might have to worry about wraparound
        public float RotateTo(float target) {
            return this.RotateTo(this.body.rotation, target);
        }

        private float RotateTo(float previous, float next) {
            this.body.rotation = next;
            int hits = this.Cast(Vector2.zero, this.hitBuffer, 0.0f);
            if (hits == 0) {
                return next;
            } else {
                float found = this.SearchRotation(previous, next, 1);
                return found;
            }
        }

        private float SearchRotation(float from, float to, int tries) {
            float mid = (from + to) / 2.0f;
            this.body.rotation = mid;
            int hits = this.Cast(Vector2.zero, this.hitBuffer, 0.0f);
            if (hits == 0 && tries == 0) {
                // if good, and no more trying, stop here
                return mid;
            } else if (hits == 0 && tries != 0) {
                // if good, but we can try more, try to pin it down better
                return this.SearchRotation(mid, to, tries - 1);
            } else if (hits != 0 && tries != 0) {
                // if hit, and we can try more, try lower
                return this.SearchRotation(from, mid, tries - 1);
            } else {
                // if hit, and we can't try more, go with lowest allowable
                return from;
            }
        }

        public void MoveUnchecked(Vector2 movement) {
            this.body.position += movement;
        }

        public bool MoveAndCollide(Vector2 movement) {
            return this.MoveAndCollide(movement, out _);
        }

        public int Cast(Vector2 movement, LayerMask mask) {
            ContactFilter2D contactFilter = new ContactFilter2D {
                useTriggers = false,
                layerMask = mask,
                useLayerMask = true
            };
            var result = this.body.Cast(movement.normalized, contactFilter, this.hitBuffer, movement.magnitude);
            return result;
        }

        public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] buffer, float distance) {
            return this.body.Cast(direction, contactFilter, buffer, distance);
        }

        public int Cast(Vector2 direction, LayerMask mask, RaycastHit2D[] buffer, float distance) {
            ContactFilter2D contactFilter = new ContactFilter2D {
                useTriggers = false,
                layerMask = mask,
                useLayerMask = true
            };
            return this.body.Cast(direction, contactFilter, buffer, distance);
        }

        private int Cast(Vector2 direction, RaycastHit2D[] buffer, float distance) {
            return this.Cast(direction, this.contactFilter, buffer, distance);
        }

        private static Vector2 RetreatMotionAlongNormal(Vector2 from, Vector2 to, Vector2 normal, float margin) {
            Vector2 motion = to - from;
            Vector2 scaledNormal = normal * margin;
            float dot = Vector2.Dot(scaledNormal, motion);
            if (dot == 0.0f) {
                return motion;
            } else {
                Vector2 retreat = (scaledNormal.sqrMagnitude / dot) * motion;
                return motion + retreat;
            }
        }

        public bool MoveAndCollide(Vector2 movement, out Collision collision) {
            RaycastHit2D[] hitBuffer = this.hitBuffer;
            Vector2 direction = movement.normalized;
            float distance = movement.magnitude;

            int hits = this.body.Cast(direction, this.contactFilter, hitBuffer);

            if (hits > 0) {
                RaycastHit2D hit = hitBuffer[0];
                Vector2 position = this.body.position;
                Vector2 target = position + Vector2.ClampMagnitude(RetreatMotionAlongNormal(position, hit.centroid, hit.normal, this.margin), distance);
                Vector2 actualMotion = target - position;
                Vector2 remainder = movement - actualMotion;

                body.position = target;

                collision = default;
                collision.normal = hit.normal;
                collision.point = target;
                collision.motion = actualMotion;
                collision.remainder = remainder;

                return Mathf.Abs(remainder.sqrMagnitude) >= Tiny;

            } else {
                Vector2 target = this.body.position + movement;
                this.body.position = target;

                collision = default;
                collision.normal = Vector2.zero;
                collision.point = target;
                collision.motion = movement;
                collision.remainder = Vector2.zero;

                return false;
            }
        }

        public Vector2 MoveAndSlide(Vector2 movement) {
            return this.MoveAndSlide(movement, this.gravity);
        }


        public Vector2 MoveAndSlide(Vector2 movement, Vector2 gravityDirection) {

            Assert.IsTrue(gravityDirection.normalized == gravityDirection);

            if (movement == Vector2.zero) {
                this.Velocity = Vector2.zero;
                return Vector2.zero;
            }

            Vector2 normalizedMovement = movement.normalized;
            Vector2 initialPosition = this.body.position;
            Vector2 remainingMotion = movement;

            IsOnFloor = false;
            IsOnCeiling = false;
            IsOnWall = false;
            FloorNormal = Vector2.zero;
            // floor body
            // floor velocity
            // colliders

            for (int slide = 0; slide < this.maxSlides; slide++) {
                Collision collision;
                bool collided = this.MoveAndCollide(remainingMotion, out collision);

                if (!collided) {
                    remainingMotion = Vector2.zero;
                } else {
                    remainingMotion = collision.remainder;

                    if (gravityDirection == Vector2.zero) {
                        // Everything is walls when you have no gravity
                        IsOnWall = true;
                    } else {
                        if (Vector2.Dot(collision.normal, -gravityDirection) > this.minSlopeDotProduct) {
                            IsOnFloor = true;
                            FloorNormal = collision.normal;
                            if (this.stopOnSlope) {
                                if ((normalizedMovement - gravityDirection).sqrMagnitude < 0.0001f && collision.motion.sqrMagnitude < 1.0f) {
                                    Vector2 position = this.body.position;
                                    position -= Slide(collision.motion, gravityDirection);
                                    this.body.position = position;
                                    break;
                                }
                            }
                        } else if (Vector2.Dot(collision.normal, gravityDirection) > this.minSlopeDotProduct) {
                            IsOnCeiling = true;
                        } else {
                            IsOnWall = true;
                        }
                    }

                    remainingMotion = Slide(remainingMotion, collision.normal);
                }

                if (!collided || remainingMotion == Vector2.zero)
                    break;
            }

            Vector2 finalPosition = this.body.position;
            Vector2 velocity = (finalPosition - initialPosition) / Time.deltaTime;
            this.Velocity = velocity;
            return velocity;
        }

        private static Vector2 Slide(Vector2 vector, Vector2 normal) {
            return vector - (normal * Vector2.Dot(vector, normal));
        }

    }

}
