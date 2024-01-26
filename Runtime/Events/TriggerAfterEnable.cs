using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Atelier.Events {

    /// <summary>
    /// Triggers a unity event (or events) a certain amount of time after an object has been 
    /// enabled.
    /// </summary>
    [AddComponentMenu("Atelier/Events/Trigger After Enable")]
    public class TriggerAfterEnable : MonoBehaviour {

        [System.Serializable]
        private struct Trigger {
            public float time;
            public UnityEvent action;
        }

        [SerializeField]
        private Trigger[] triggers;


        private void OnEnable() {
            foreach (var trigger in this.triggers) {
                if (trigger.time == 0.0f) {
                    trigger.action.Invoke();
                } else {
                    StartCoroutine(this.DelayedInvoke(trigger));
                }
            }
        }


        private void OnDisable() {
            this.StopAllCoroutines();
        }

        private IEnumerator DelayedInvoke(Trigger trigger) {
            yield return new WaitForSeconds(trigger.time);
            trigger.action.Invoke();
        }

    }



}

