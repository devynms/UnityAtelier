using UnityEngine;

namespace Atelier.Core {

    /// <summary>
    /// Adding gizmos via component to help with debugging. I tend to stick this on things like
    /// invisible triggers and whatnot.
    /// </summary>
    [AddComponentMenu("Atelier/Core/Sphere Gizmo")]
    public class SphereGizmo : MonoBehaviour {

        public enum DrawMode {
            OnDraw,
            OnSelected,
            Always,
        }

        [SerializeField]
        private DrawMode drawMode;

        [SerializeField]
        private float radius;

        [SerializeField]
        private Color color;

        private void OnDrawGizmos() {
            if (this.drawMode != DrawMode.OnSelected) {
                this.DrawGizmo();
            }
        }

        private void OnDrawGizmosSelected() {
            if (this.drawMode != DrawMode.OnDraw) {
                this.DrawGizmo();
            }
        }

        private void DrawGizmo() {
            Gizmos.color = this.color;
            Gizmos.DrawSphere(this.transform.position, this.radius);
        }

    }

}

