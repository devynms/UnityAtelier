using Atelier.Containers;
using UnityEngine;

namespace Atelier.Cameras {

    // Janky version of unity constraints using container stuff

    [AddComponentMenu("Atelier/Cameras/Game Object Tracker")]
    public class GameObjectTracker : MonoBehaviour {

        [SerializeField]
        private GameObjectContainer container;

        private GameObject cachedObject;

        public void Update() {
            if (this.cachedObject == null) {
                GameObject obj = this.container.Object;
                this.cachedObject = obj;
            }
            if (this.cachedObject != null) {
                Vector3 objPosition = this.cachedObject.transform.position;
                this.transform.position = new Vector3 {
                    x = objPosition.x,
                    y = objPosition.y,
                    z = this.transform.position.z,
                };
            }
        }

    }

}

