using UnityEngine;
using UnityEngine.SceneManagement;

namespace Atelier.Scenes {

    /// <summary>
    /// Component-ey version of changing to a target scene. Mostly used as a target for unity
    /// events.
    /// </summary>
    [AddComponentMenu("Atelier/Scenes/Scene Changer")]
    public class SceneChanger : MonoBehaviour {

        [Scene]
        [SerializeField]
        private string scene;

        public void ChangeScene() {
            SceneManager.LoadScene(this.scene);
        }

    }

}

