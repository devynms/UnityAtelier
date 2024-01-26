using UnityEngine;

namespace Atelier.Eightway {

    /// <summary>
    /// Attach an eight-way direction to an object.
    /// </summary>
    [AddComponentMenu("Atelier/Eightway/Eightway Facing")]
    public class EightwayFacing : MonoBehaviour {

        [SerializeField]
        private EightwayDirection direction;

        public EightwayDirection Direction {
            get => this.direction;
            set => this.direction = value;
        }

    }

}

