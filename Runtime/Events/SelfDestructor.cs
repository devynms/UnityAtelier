using UnityEngine;

namespace Atelier.Events {


    /// <summary>
    /// Helper for destroying an object through unity events. If there was a built-in way to do
    /// this, I aplogize.
    /// 
    /// SelfDestrucotr.Destruct() can be the target of an event.
    /// </summary>
    [AddComponentMenu("Atelier/Events/Self Destructor")]
    public class SelfDestructor : MonoBehaviour {

        public void Destruct() {
            Destroy(this.gameObject);
        }

    }

}

