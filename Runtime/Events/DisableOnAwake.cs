using UnityEngine;

namespace Atelier.Events {

    /// <summary>
    /// Not actually an event-helper, but this seemed like the best place to put it.
    /// 
    /// Apparently used to only enable certain objects when debugging, but I can't actually find any
    /// examples of how I used it...
    /// </summary>
    [AddComponentMenu("Atelier/Events/Disable On Awake")]
    public class DisableOnAwake : MonoBehaviour {

        [SerializeField]
        private bool debugging = false;

        private void Awake() {
            this.gameObject.SetActive(this.debugging);
        }

        private void OnValidate() {
            if (Application.isPlaying) {
                this.gameObject.SetActive(this.debugging);
            } else {
                this.gameObject.SetActive(true);
            }
        }

    }

}

