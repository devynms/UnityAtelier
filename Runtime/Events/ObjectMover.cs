using UnityEngine;

namespace Atelier.Events {

    /// <summary>
    /// Meant as a receiver for moving objects onto the position of others via unity events.
    /// 
    /// Seems a little overly-specific, now that I'm here...
    /// </summary>
    [AddComponentMenu("Atelier/Events/Object Mover")]
    public class ObjectMover : MonoBehaviour {

        public void MoveObject(GameObject message) {
            this.transform.position = message.transform.position;
        }

    }

}

