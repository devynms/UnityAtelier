using UnityEngine;

namespace Atelier.Core {

    /// <summary>
    /// Scriptable object based tags.
    /// 
    /// I'm honestly not sure why I built this, my reference project seems to just use normal game
    /// object tags.
    /// </summary>
    [CreateAssetMenu(fileName = "NewTag", menuName = "Atelier/Tag")]
    public class Tag : ScriptableObject {
    }

}

