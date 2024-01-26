using System;
using UnityEngine;
using UnityEngine.Events;

namespace Atelier.Physics {

    public struct PlatformCollision {
        public KinematicPlatform platform;
        public RaycastHit2D hit;
        public Vector2 movement;

        public override string ToString() {
            return string.Format(
                "PlatformCollision {{ platform = {0}, hit = {1}, movement = {2} }}",
                this.platform, HitToString(this.hit), this.movement
            );
        }

        private static string HitToString(RaycastHit2D hit) {
            return string.Format(
                "RaycastHit2D {{ centroid = {0}, collider = {1}, distance = {2}, fraction = {3}, normal = {4}, point = {5}, rigidbody = {6}, transform = {7}",
                hit.centroid, hit.collider, hit.distance, hit.fraction, hit.normal, hit.point, hit.rigidbody, hit.transform
            );
        }
    }

    public struct PlatformMovement {
        public KinematicPlatform platform;
        public Vector2 movement;

        public override string ToString() {
            return string.Format(
                "PlatformMovement {{ platform = {0}, movement = {1} }}",
                this.platform, this.movement
            );
        }
    }

    [Serializable]
    public class PlatformCollisionEvent : UnityEvent<PlatformCollision> {
    }

    [Serializable]
    public class PlatformMovementEvent : UnityEvent<PlatformMovement> {
    }


    /// <summary>
    /// Receiver for game objects that want to receive platform collision/movement events.
    /// 
    /// Honestly might not be the best take on this. Maybe in an ideal world, it would behave a bit
    /// more like the built-in events (OnTriggerEntered, etc.) do, in that a controller could
    /// implement those methods and it would "just work".
    /// 
    /// Are these sorts of callbacks/events even the best way to implement objects sticking to a
    /// platform?
    /// 
    /// Not sure honestly, I found moving platforms kind of tricky.
    /// </summary>
    [RequireComponent(typeof(KinematicBody2D))]
    public class KinematicMobile : MonoBehaviour {

        private KinematicBody2D body;

        [SerializeField]
        private PlatformCollisionEvent onPlatformCollision;

        [SerializeField]
        private PlatformMovementEvent onPlatformMovement;

        public Vector2 Position => this.body.Position;

        private void Awake() {
            this.body = GetComponent<KinematicBody2D>();
        }

        public void CollideWithPlatform(PlatformCollision collision) {
            this.onPlatformCollision.Invoke(collision);
        }

        public void MoveWithPlatform(PlatformMovement movement) {
            this.onPlatformMovement.Invoke(movement);
        }

    }

}

