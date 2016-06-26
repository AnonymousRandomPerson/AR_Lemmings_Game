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
            if (!visited) {
                visited = true;
                SurfaceManager.instance.VisitSurface();
            }
        }
    }
}
