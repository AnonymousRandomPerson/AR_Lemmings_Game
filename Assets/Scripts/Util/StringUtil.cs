using UnityEngine;

namespace Lemmings.Util {

    /// <summary>
    /// Utility methods for string manipulation.
    /// </summary>
    class StringUtil {

        /// <summary>
        /// Removes " (Clone)" from a spawned game object.
        /// </summary>
        /// <returns>The name of the game object with " (Clone)" removed.</returns>
        /// <param name="name">The name of the game object.</param>
        public static string RemoveClone(string name) {
            return name.Substring(0, name.Length - 7);
        }
    }
}