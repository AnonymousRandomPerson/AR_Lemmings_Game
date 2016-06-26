using UnityEngine;

namespace Lemmings.Util {
    /// <summary>
    /// Prints the position of the object in JSON format.
    /// </summary>
    class PointMarker : MonoBehaviour {

        /// <summary> The JSON array created by the point markers. </summary>
        private static string jsonArray = "";

        /// <summary>
        /// Prints the position of the object in JSON format.
        /// </summary>
        private void Start() {
            jsonArray += "                    [" + transform.position.x + "," + transform.position.y + "," + transform.position.z + "],\n";
            Debug.Log(jsonArray);
        }
    }
}
