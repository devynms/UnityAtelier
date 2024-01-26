using UnityEngine;

namespace Atelier.Messages {

    /// <summary>
    /// Send messages on trigger enter.
    /// 
    /// Originally written for 2d-only. Apologies if the 3-d code broke this.
    /// </summary>
    [AddComponentMenu("Atelier/Messages/Send Message On Trigger Enter")]
    public class SendMessageOnTriggerEnter : MonoBehaviour {

#if UNITY_EDITOR
        [SerializeField]
        private bool enableLogging = false;
#else
        private bool enableLogging => false;
#endif

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private MessageType messageType;

        [SerializeField]
        private GameObject messageContainer;

        private void OnTriggerEnter2D(Collider2D collision) {
            this.SendMessage(collision.gameObject, collision.attachedRigidbody.gameObject);
        }

        private void OnTriggerEnter(Collider collision) {
            this.SendMessage(collision.gameObject, collision.attachedRigidbody.gameObject);
        }

        private void SendMessage(GameObject colliderObject, GameObject rigidbodyObject) {
            if (this.LayerMaskContains(colliderObject.layer)) {
                MessageReceiver receiver = rigidbodyObject.gameObject.GetComponent<MessageReceiver>();
                if (receiver != null && receiver.Receives(this.messageType)) {
                    this.LogFormat("Sending message to receiver {0}", receiver);
                    receiver.Send(this.messageType, this.messageContainer);
                } else {
                    this.LogFormat("Invalid receiver {0}", receiver);
                }
            }
        }


        private bool LayerMaskContains(int layer) {
            return (this.LayerToMask(layer) & this.layerMask.value) != 0;
        }

        private int LayerToMask(int layer) {
            return 1 << layer;
        }

        private void LogFormat(string format, params object[] args) {
            if (this.enableLogging) {
                Debug.LogFormat(format, args);
            }
        }

    }

}
