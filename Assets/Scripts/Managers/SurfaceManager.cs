using UnityEngine;
using System.Collections.Generic;
using Lemmings.Level;

namespace Lemmings.Managers {
    /// <summary>
    /// Keeps track of surfaces in the environment.
    /// </summary>
    class SurfaceManager : MonoBehaviour {
        
        /// <summary> The singleton instance of the surface manager. </summary>
        private static SurfaceManager _instance;
        /// <summary> The singleton instance of the surface manager. </summary>
        public static SurfaceManager instance {
            get {
                return _instance;
            }
        }

        /// <summary> The surfaces in the environment. </summary>
        private List<Surface> surfaces;

        /// <summary> The number of surfaces that have been visited. </summary>
        private int _numVisitedSurfaces;
        /// <summary> The number of surfaces that have been visited. </summary>
        public int numVisitedSurfaces {
            get { return _numVisitedSurfaces; }
        }

        /// <summary>
        /// Sets the singleton instance of the surface manager.
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
            _numVisitedSurfaces++;
        }

        /// <summary>
        /// Resets the visited surfaces.
        /// </summary>
        internal void Reset() {
            _numVisitedSurfaces = 0;
            foreach (Surface surface in surfaces) {
                surface.Reset();
            }
        }

        /// <summary>
        /// Sets whether the surfaces are visible.
        /// </summary>
        /// <param name="visible">Whether the surfaces are visible.</param>
        public void SetVisible(bool visible) {
            foreach (Surface surface in surfaces) {
                surface.SetVisible(visible);
            }
        }

        /// <summary>
        /// Gets the surface with a certain index.
        /// </summary>
        /// <returns>The surface with the specified index.</returns>
        /// <param name="index">The index of the surface to get.</param>
        public Surface GetSurface(int index) {
            return surfaces[index];
        }
    }
}