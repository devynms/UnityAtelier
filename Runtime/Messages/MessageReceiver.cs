using System;
using UnityEngine;
using UnityEngine.Events;

namespace Atelier.Messages {

    [Serializable]
    public class EventReceiver : UnityEvent<GameObject> { }

    /// <summary>
    /// Designates a unity event as a receiver for a specific message type.
    /// </summary>
    [Serializable]
    public struct MessageBox {

        [SerializeField]
        private MessageType messageType;

        [SerializeField]
        private EventReceiver receiver;

        public MessageType MessageType { get => this.messageType; }

        public UnityEvent<GameObject> Receiver { get => this.receiver; }

    }

    /// <summary>
    /// When a message is sent to an object via its message reciever, the receiver dispatches to a
    /// specific unity event depending on the message type.
    /// </summary>
    [AddComponentMenu("Atelier/Messages/Message Receiver")]
    public class MessageReceiver : MonoBehaviour {

#if UNITY_EDITOR
        [SerializeField]
        private bool enableLogging = false;
#else
        private bool enableLogging => false;
#endif

        [SerializeField]
        private MessageBox[] receivers;

        public bool Receives(MessageType messageType) {
            foreach (MessageBox receiver in this.receivers) {
                if (receiver.MessageType == messageType)
                    return true;
            }
            return false;
        }

        public void Send(MessageType type, GameObject container) {
            foreach (MessageBox receiver in this.receivers) {
                if (receiver.MessageType == type) {
                    this.LogFormat("Forwarding message {0} from {1} to receiver {2}", type, container, receiver);
                    receiver.Receiver.Invoke(container);
                }
            }
        }

        private void LogFormat(string format, params object[] args) {
            if (this.enableLogging) {
                Debug.LogFormat(format, args);
            }
        }

    }

}
