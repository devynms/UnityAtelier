using UnityEngine;


namespace Atelier.Containers {

    /// <summary>
    /// Registrar for assigning a specific game object to a static scriptable object. Used
    /// primarily for creating indirect references to other objects in the scene that work with
    /// prefabs.
    /// </summary>
    [AddComponentMenu("Atelier/Containers/Game Object Registrar")]
    public class GameObjectRegistrar : MonoBehaviour {

        [SerializeField]
        private GameObjectContainer container;

        private void OnEnable() {
            this.container.Register(this.gameObject);
        }

        private void OnDisable() {
            this.container.Deregister(this.gameObject);
        }

    }

}
