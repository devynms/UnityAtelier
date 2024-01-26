using UnityEngine;
using UnityEngine.Events;

namespace Atelier.Events {

    [System.Serializable]
    public struct TimedEvent {
        [SerializeField] public float time;
        [SerializeField] public float noise;
        [SerializeField] public UnityEvent action;

        public float mark;
    }

    /// <summary>
    /// Trigger a timed sequence of multiple events.
    /// Can serve as the target of another event.
    /// </summary>
    [AddComponentMenu("Atelier/Events/Timed Sequence")]
    public class TimedSequence : MonoBehaviour {

        [SerializeField]
        private TimedEvent[] sequence;

        private float timer = 0.0f;
        private bool running = false;

        public void StartEvents() {
            this.timer = 0.0f;
            this.running = true;
            this.CreateNoise();
        }

        private void CreateNoise() {
            for (int i = 0; i < this.sequence.Length; i++) {
                TimedEvent ev = this.sequence[i];
                this.sequence[i].mark = ev.time + Random.Range(-ev.noise, ev.noise);
            }
        }

        private void FixedUpdate() {
            if (!this.running) return;
            float previousTime = this.timer;
            float nextTime = previousTime + Time.deltaTime;
            bool eventsRemaining = false;
            foreach (TimedEvent ev in this.sequence) {
                if (ev.mark >= previousTime && ev.mark < nextTime) {
                    ev.action.Invoke();
                }
                if (ev.mark >= nextTime) {
                    eventsRemaining = true;
                }
            }
            this.timer = nextTime;
            this.running = eventsRemaining;
        }

    }

}

