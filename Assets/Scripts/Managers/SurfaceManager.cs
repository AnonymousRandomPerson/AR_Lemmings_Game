using UnityEngine;
using System.Collections.Generic;
using Lemmings.Level;

namespace Lemmings.Managers {
    /// <summary>
    /// Keeps track of surfaces in the environment.
    /// </summary>
    class SurfaceManager : MonoBehaviour {
        
        /// <summary> The singleton instance of the game manager. </summary>
        private static SurfaceManager _instance;
        /// <summary> The singleton instance of the game manager. </summary>
        public static SurfaceManager instance {
            get {
                return _instance;
            }
        }

        /// <summary> The surfaces in the environment. </summary>
        private List<Surface> surfaces;

        /// <summary> The number of surfaces that have been visited. </summary>
        private int numVisitedSurfaces;

        /// <summary>
        /// Sets the singleton instance of the game manager.
        /// </summary>
        private void Awake() {
            _instance = this;
            surfaces = new List<Surface>();
        }

        /// <summary>
        /// Adds a surface to the surface manager.
        /// </summary>
        /// <param name="surface">The surface to add.</param>
        public void AddSurface(Surface surface) {
            surfaces.Add(surface);
        }

        /// <summary>
        /// Registers a visited surface.
        /// </summary>
        public void VisitSurface() {
            numVisitedSurfaces++;
        }

        /// <summary>
        /// Resets the visited surfaces.
        /// </summary>
        internal void Reset() {
            numVisitedSurfaces = 0;
            foreach (Surface surface in surfaces) {
                surface.Reset();
            }
        }
    }
}