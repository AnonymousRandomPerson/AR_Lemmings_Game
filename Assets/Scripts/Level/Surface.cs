using UnityEngine;
using Lemmings.Managers;

namespace Lemmings.Level {
    /// <summary>
    /// A distinct object in the environment.
    /// </summary>
    class Surface : MonoBehaviour {

        /// <summary> Whether the surface has been visited. </summary>
        [HideInInspector]
        public bool visited;

        /// <summary> Whether the surface is part of the floor. </summary>
        [HideInInspector]
        public bool isFloor;

        /// <summary>
        /// Marks the surface as unvisited.
        /// </summary>
        public void Reset() {
            visited = false;
        }

        /// <summary>
        /// Marks the surface as visited.
        /// </summary>
        public void VisitSurface() {
            if (!visited && !isFloor) {
                visited = true;
                SurfaceManager.instance.VisitSurface();
            }
        }

        /// <summary>
        /// Sets whether the surface is visible.
        /// </summary>
        /// <param name="visible">Whether the surface is visible.</param>
        public void SetVisible(bool visible) {
            foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
                renderer.enabled = visible;
            }
        }
    }
}
