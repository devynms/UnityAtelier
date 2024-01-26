using System.Collections.Generic;
using UnityEngine;

namespace Atelier.Physics {

    /// <summary>
    /// Attaches to a kinematic body which is supposed to act as a platform.
    /// </summary>
    [AddComponentMenu("Atelier/Physics/Kinematic Platform")]
    [RequireComponent(typeof(KinematicBody2D))]
    public class KinematicPlatform : MonoBehaviour {

        private enum MovementType {
            Pulling,
            PushingOrSliding,
        }

        [SerializeField]
        private LayerMask mobileLayer;

        [SerializeField]
        private int maxAttachedMobiles;


        private List<KinematicMobile> attachedMobiles;
        private KinematicBody2D body;

        public Vector2 Position => this.body.Position;


        private void Awake() {
            this.attachedMobiles = new List<KinematicMobile>(this.maxAttachedMobiles);
            this.body = GetComponent<KinematicBody2D>();
        }

        public void AttachMobile(KinematicMobile mobile) {
            if (this.attachedMobiles.Count < this.attachedMobiles.Capacity) {
                this.attachedMobiles.Add(mobile);
            }
        }

        public void DetachMobile(KinematicMobile mobile) {
            this.attachedMobiles.Remove(mobile);
        }

        public void Move(Vector2 movement) {
            this.MoveAttachedMobiles(movement, MovementType.PushingOrSliding);
            this.CollideWithOtherMobiles(movement);
            this.MovePlatform(movement);
            this.MoveAttachedMobiles(movement, MovementType.Pulling);
        }

        private void CollideWithOtherMobiles(Vector2 movement) {
            int hits = this.body.Cast(movement, this.mobileLayer);
            for (int hitIndex = 0; hitIndex < hits; hitIndex++) {
                RaycastHit2D hit = this.body.HitResults[hitIndex];
                KinematicMobile mobile = hit.collider.attachedRigidbody.GetComponent<KinematicMobile>();
                if (mobile != null && !this.attachedMobiles.Contains(mobile)) { 
                    mobile.CollideWithPlatform(new PlatformCollision {
                        platform = this,
                        hit = hit,
                        movement = movement,
                    });
                }
            }
        }

        private Vector2 MovePlatform(Vector2 movement) {
            return this.body.MoveAndSlide(movement);
        }

        private void MoveAttachedMobiles(Vector2 movement, MovementType type) {
            for (int i = 0; i < this.attachedMobiles.Count; i++) {
                KinematicMobile attachedMobile = this.attachedMobiles[i];
                if (MovementIsOfType(movement, attachedMobile, type)) {
                    attachedMobile.MoveWithPlatform(new PlatformMovement {
                        platform = this,
                        movement = movement,
                    });
                }
            }
        }

        private bool MovementIsOfType(Vector2 movement, KinematicMobile mobile, MovementType type) {
            Vector2 position = this.Position;
            float sqrCurrentDistance = (position - mobile.Position).sqrMagnitude;
            float sqrPostDistance = (position + movement - mobile.Position).sqrMagnitude;
            bool movingAway = sqrPostDistance > sqrCurrentDistance;
            return (movingAway && type == MovementType.Pulling) || (!movingAway && type == MovementType.PushingOrSliding);
        }

    }

}

