using UnityEngine;
using UnityEngine.Events;

namespace Atelier.Events {

    /// <summary>
    /// Trigger a child event after a delay. Sort of a simpler version of TimedSequence.
    /// 
    /// Can be the target of a unity event.
    /// </summary>
    [AddComponentMenu("Atelier/Events/Trigger After Delay")]
    public class TriggerAfterDelay : MonoBehaviour {

        [SerializeField]
        private float delay;

        [SerializeField]
        private UnityEvent trigger;

        private float timer;
        private float targetTime;

        private bool triggerSet = false;

        public void Trigger() {
            this.Trigger(this.delay);
        }

        public void Trigger(float delay) {
            this.targetTime = delay;
            this.timer = 0.0f;
            this.triggerSet = true;
        }

        public void FixedUpdate() {
            if (this.triggerSet) {
                this.timer += Time.deltaTime;
                if (this.timer >= this.targetTime) {
                    this.trigger.Invoke();
                    this.triggerSet = false;
                }
            }
        }

    }

}

