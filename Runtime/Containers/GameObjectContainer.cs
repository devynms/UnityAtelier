using UnityEngine;


namespace Atelier.Containers {

    /// <summary>
    /// Scriptable object, designed to decouple prefabs from direct references to another object 
    /// in the scene. For example: when creating a prefab that references the player character game
    /// object, you can create a Player container. Then, the player prefab simply adds a registrar
    /// component which references the same scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "NewGameObjectContainer", menuName = "Atelier/Game Object Container")]
    public class GameObjectContainer : ScriptableObject {

        public delegate void ContainerEventHandler(GameObject obj);

        public GameObject Object { get; private set; }

        private event ContainerEventHandler onContainerRegister;
        private event ContainerEventHandler onContainerDeregister;

        public T GetComponent<T>() where T : MonoBehaviour {
            if (this.Object == null) {
                Debug.LogFormat(
                    "{0} requested from container {1} with no object.",
                    typeof(T).Name, this
                );
                return null;
            }
            T component = this.Object.GetComponent<T>();
            Debug.AssertFormat(
                component != null,
                "{0} requested from object {1} in container {2} with no such component.",
                typeof(T).Name, this.Object, this
            );
            return component;
        }

        public void AddRegisterHandler(ContainerEventHandler handler) {
            this.onContainerRegister += handler;
            if (this.Object != null) {
                handler.Invoke(this.Object);
            }
        }

        public void RemoveRegisterHandler(ContainerEventHandler handler) {
            this.onContainerRegister -= handler;
        }

        public void AddDeregisterHandler(ContainerEventHandler handler) {
            this.onContainerDeregister += handler;
        }

        public void RemoveDeregisterHandler(ContainerEventHandler handler) {
            this.onContainerDeregister -= handler;
        }

        public void Register(GameObject obj) {
            Debug.AssertFormat(
                this.Object == null || this.Object == obj,
                "{0} should only have an object registered once.\nAlready had: {1} [scene: {3}].\nTried to set: {2} [scene: {4}].",
                this, this.Object, obj, this.Object?.scene.name, obj.scene.name
            );
            this.Object = obj;
            this.onContainerRegister?.Invoke(obj);
        }

        public void Deregister(GameObject obj) {
            if (obj == this.Object) {
                this.Object = null;
                this.onContainerDeregister?.Invoke(obj);
            }
        }

        private void OnEnable() {
            this.Object = null;
        }

        private void OnDisable() {
            this.Object = null;
            this.onContainerRegister = null;
            this.onContainerDeregister = null;
        }

    }

}
