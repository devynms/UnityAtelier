using Atelier.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Atelier.Events {


    [System.Serializable]
    public class TriggerEvent : UnityEvent<GameObject> { }

    /// <summary>
    /// Helper for triggering unity events via OnTriggerEnter + OnTriggerEnter2D.
    /// 
    /// Originally just created with OnTriggerEnter2D, so I hope adding OnTriggerEnter doesn't break
    /// anything...
    /// </summary>
    [AddComponentMenu("Shrl/Core/Trigger On Enter")]
    public class TriggerOnEnter : MonoBehaviour {

        [SerializeField]
        private LayerMask mask;

        [SerializeField]
        private TriggerEvent trigger;

        private void OnTriggerEnter2D(Collider2D collision) {
            GameObject obj = collision.attachedRigidbody.gameObject;
            int layer = obj.layer;
            if (this.mask.ContainsLayer(layer)) {
                this.trigger.Invoke(obj);
            }
        }

        private void OnTriggerEnter(Collider collision) {
            GameObject obj = collision.attachedRigidbody.gameObject;
            int layer = obj.layer;
            if (this.mask.ContainsLayer(layer)) {
                this.trigger.Invoke(obj);
            }
        }

    }

}

