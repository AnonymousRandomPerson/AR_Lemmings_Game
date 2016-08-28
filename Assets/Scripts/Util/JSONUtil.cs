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

        /// <summary>
        /// Gets a JSON float field if it exists.
        /// </summary>
        /// <returns>The field if it exists, or the default value if the field doesn't exist.</returns>
        /// <param name="json">The JSON object to parse.</param>
        /// <param name="field">The field to get from the JSON.</param>
        /// <param name="defaultValue">The default value if the field doesn't exist.</param>
        public static float GetIfExists(JSONObject json, string field, float defaultValue) {
            return json.HasField(field) ? json.GetField(field).f : defaultValue;
        }
    }
}

