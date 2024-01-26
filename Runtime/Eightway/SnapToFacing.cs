using UnityEngine;

namespace Atelier.Eightway {

    [AddComponentMenu("Atelier/Eightway/Snap to Facing")]
    public class SnapToFacing : MonoBehaviour {

        [SerializeField]
        private Vector2 reference;

        private EightwayFacing facing;

        private void Awake() {
            this.facing = GetComponentInParent<EightwayFacing>();
            Debug.Assert(this.facing != null);
        }

        private void Update() {
            this.transform.eulerAngles = Vector3.forward * this.facing.Direction.AngleFrom(this.reference);
        }
    }

}

