using UnityEngine;

namespace Lemmings.Util {
    /// <summary>
    /// Utility methods for JSON parsing.
    /// </summary>
    static class JSONUtil {
        
        /// <summary>
        /// Makes a vector from a size 3 JSON array.
        /// </summary>
        /// <returns>A vector from the size 3 JSON array. </returns>
        /// <param name="json">The size 3 JSON array to create a vector from.</param>
        public static Vector3 MakeVectorFromJSON(JSONObject json) {
            Vector3 returnVector = new Vector3(json.list[0].n, json.list[1].n, json.list[2].n);
            return returnVector;
        }
    }
}

