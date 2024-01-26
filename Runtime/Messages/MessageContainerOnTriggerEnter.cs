using Atelier.Containers;
using UnityEngine;

namespace Atelier.Messages {

    /// <summary>
    /// This feels like I could have combined something in Events to acheive this but oh well...
    /// </summary>
    [AddComponentMenu("Atelier/Messages/Message Container On Trigger Enter")]
    public class MessageContainerOnTriggerEnter : MonoBehaviour {

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private bool sendOnce;

        [SerializeField]
        private MessageType messageType;

        [SerializeField]
        private GameObject messageContainer;

        [SerializeField]
        private GameObjectContainer receiverContainer;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (this.LayerMaskContains(collision.gameObject.layer)) {
                MessageReceiver receiver = this.receiverContainer.Object.GetComponent<MessageReceiver>();
                if (receiver != null && receiver.Receives(this.messageType)) {
                    this.SendMessage(receiver);
                }
            }
        }


        private void SendMessage(MessageReceiver receiver) {
            receiver.Send(this.messageType, this.messageContainer);
            if (this.sendOnce) {
                this.enabled = false;
            }
        }


        private bool LayerMaskContains(int layer) {
            return (this.LayerToMask(layer) & this.layerMask.value) != 0;
        }

        private int LayerToMask(int layer) {
            return 1 << layer;
        }

    }

}
