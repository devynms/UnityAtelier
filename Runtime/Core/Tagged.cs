using UnityEngine;

namespace Atelier.Core {

    /// <summary>
    /// Component-side system for tagging stuff with scriptable objects.
    /// 
    /// I'm honestly not sure why I built this, my reference project seems to just use normal game
    /// object tags.
    /// </summary>
    [AddComponentMenu("Atelier/Core/Tagged")]
    public class Tagged : MonoBehaviour {

        [SerializeField]
        private Tag tagObject;

        public Tag Tag => this.tagObject;
    }

}

