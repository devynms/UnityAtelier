using UnityEngine;

namespace Atelier.Events {

    [System.Serializable]
    public enum SwitchPosition {
        Open,
        Closed,
    }

    /// <summary>
    /// Somewhat handier than "ObjectMover", this component lets one trigger an object to move
    /// in a straight line between points determined by two other transforms.
    /// 
    /// Those transforms are often just two other dummy objects with a SphereGizmo stuck on them.
    /// 
    /// Be careful not to make the target transforms children of the object you're moving. As a
    /// quick solution, you can create a dummy object that parents the moving object and the target
    /// positions as siblings.
    /// </summary>
    [AddComponentMenu("Atelier/Events/Switch Rail Mover")]
    public class SwitchRailMover : MonoBehaviour {

        [SerializeField]
        private SwitchPosition switchPosition;

        [SerializeField]
        private Transform openPosition;

        [SerializeField]
        private Transform closedPosition;

        [SerializeField]
        private float animationTime;


        private float timer;
        private float invAnimationTime;


        private float backwardsTimer => this.animationTime - this.timer;
        private float openClosedTime => this.backwardsTimer;
        private float closedOpenTime => this.timer;
        private float scale => (this.switchPosition == SwitchPosition.Open ? this.openClosedTime : this.closedOpenTime) * this.invAnimationTime;


        private void Awake() {
            this.timer = this.animationTime;
            this.invAnimationTime = 1.0f / this.animationTime;
        }

        private void FixedUpdate() {
            this.timer = Mathf.Clamp(this.timer + Time.deltaTime, 0.0f, this.animationTime);
            this.transform.position = Vector3.Lerp(this.openPosition.position, this.closedPosition.position, this.scale);
        }


        public void FlipSwitch() {
            if (this.switchPosition == SwitchPosition.Open) {
                this.switchPosition = SwitchPosition.Closed;
            } else {
                this.switchPosition = SwitchPosition.Open;
            }
            this.timer = this.backwardsTimer;
        }

        public void SetOpen() {
            if (this.switchPosition == SwitchPosition.Closed) {
                this.FlipSwitch();
            }
        }

        public void SetClosed() {
            if (this.switchPosition == SwitchPosition.Open) {
                this.FlipSwitch();
            }
        }

        public void SetSwitch(SwitchPosition position) {
            if (this.switchPosition != position) {
                this.FlipSwitch();
            }
        }

    }

}

