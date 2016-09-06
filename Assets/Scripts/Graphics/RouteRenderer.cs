using UnityEngine;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.Graphics {
    /// <summary>
    /// Renders the intended surface route.
    /// </summary>
    class RouteRenderer : MonoBehaviour {

        /// <summary> The singleton instance of the route renderer. </summary>
        private static RouteRenderer _instance;
        /// <summary> The singleton instance of the route renderer. </summary>
        public static RouteRenderer instance {
            get {
                return _instance;
            }
        }

        /// <summary> The game object that holds the route lines. </summary>
        private GameObject routeContainer;

        /// <summary> Whether route lines are visible. </summary>
        [Tooltip("Whether route lines are visible.")]
        public bool visible;

        /// <summary> The material used to render the route. </summary>
        [SerializeField]
        [Tooltip("The material used to render the route.")]
        private Material routeMaterial;

        /// <summary> The width of the route. </summary>
        [SerializeField]
        [Tooltip("The width of the route.")]
        private float routeWidth;

        /// <summary>
        /// Sets the singleton instance of the route renderer.
        /// </summary>
        private void Awake() {
            _instance = this;
        }

        /// <summary>
        /// Sets the route as visible or invisible depending on the setting.
        /// </summary>
        private void Update() {
            if (routeContainer != null) {
                routeContainer.SetActive(visible);
            }
        }

        /// <summary>
        /// Draws the surface route.
        /// </summary>
        /// <param name="goalPosition">The position of the lemming spawn point.</param>
        /// <param name="route">The surfaces in the route.</param>
        /// <param name="goalPosition">The position of the goal.</param>
        public void DrawRoute(Vector3 startPosition, int[] route, Vector3 goalPosition) {
            if (visible) {
                if (routeContainer == null) {
                    routeContainer = ObjectUtil.CreateNewObject("Route");
                }
                for (int i = -1; i < route.Length; i++) {
                    GameObject lineObject = ObjectUtil.CreateNewObject("Line", routeContainer);
                    LineRenderer line = lineObject.AddComponent<LineRenderer>();
                    Vector3 start = i >= 0 ? GetSurfaceCenter(route[i]) : startPosition;
                    Vector3 end = i < route.Length - 1 ? GetSurfaceCenter(route[i + 1]) : goalPosition;
                    Vector3[] linePositions = new Vector3[]{start, end};
                    for (int j = 0; j < linePositions.Length; j++) {
                        line.SetPosition(j, linePositions[j]);
                    }
                    line.SetVertexCount(linePositions.Length);

                    line.material = routeMaterial;
                    line.SetWidth(routeWidth, routeWidth);
                }
            }
        }

        /// <summary>
        /// Gets the center of a surface.
        /// </summary>
        /// <returns>The center of the specified surface.</returns>
        /// <param name="index">The index of the surface to get.</param>
        private Vector3 GetSurfaceCenter(int index) {
            return SurfaceManager.instance.GetSurface(index).center;
        }
    }
}
