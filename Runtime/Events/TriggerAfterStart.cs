using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Atelier.Events {

    /// <summary>
    /// Triggers a unity event (or events) a certain amount of time after an object has been 
    /// started.
    /// </summary>
    [AddComponentMenu("Atelier/Events/Trigger After Start")]
    public class TriggerAfterStart : MonoBehaviour {

        [System.Serializable]
        private struct Trigger {
            public float time;
            public UnityEvent action;
        }

        [SerializeField]
        private Trigger[] triggers;


        private void Awake() {
            foreach (var trigger in this.triggers) {
                StartCoroutine(this.DelayedInvoke(trigger));
            }
        }

        private IEnumerator DelayedInvoke(Trigger trigger) {
            yield return new WaitForSeconds(trigger.time);
            trigger.action.Invoke();
        }

    }

}

