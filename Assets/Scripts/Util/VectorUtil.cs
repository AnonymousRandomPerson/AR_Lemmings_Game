using UnityEngine;

namespace Lemmings.Util {
    /// <summary>
    /// Utility methods for manipulating vectors.
    /// </summary>
    static class VectorUtil {

        /// <summary>
        /// Sets the y component of a vector.
        /// </summary>
        /// <returns>A new vector with a modified y component from the previous vector.</returns>
        /// <param name="vector">The vector to set the y component of</param>
        /// <param name="height">The y component to set the vector to.</param>
        public static Vector3 SetY(Vector3 vector, float y) {
            Vector3 newVector = vector;
            newVector.y = y;
            return newVector;
        }

        /// <summary>
        /// Gets a vector orthonormal to a vector in the xz plane.
        /// </summary>
        /// <returns>A vector orthonormal to the given vector in the xz plane.</returns>
        /// <param name="vector">The vector to get an orthonormal vector from.</param>
        public static Vector3 GetOrthonormal(Vector3 vector) {
            Vector3 newVector = vector.normalized;
            newVector = new Vector3(-newVector.z, newVector.y, newVector.x);
            return newVector;
        }
    }
}

