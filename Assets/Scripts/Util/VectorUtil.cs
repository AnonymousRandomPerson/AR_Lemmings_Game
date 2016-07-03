using UnityEngine;

namespace Lemmings.Util {
    /// <summary>
    /// Utility methods for manipulating vectors.
    /// </summary>
    static class VectorUtil {

        /// <summary>
        /// Sets the x and z components of a vector.
        /// </summary>
        /// <returns>The modified vector.</returns>
        /// <param name="vector">The vector to modify.</param>.</param>
        /// <param name="number">The number to set the x and z components to.</param>
        public static Vector3 SetXZ(Vector3 vector, float number) {
            Vector3 newVector = vector;
            newVector.x = number;
            newVector.z = number;
            return newVector;
        }

        /// <summary>
        /// Sets the y component of a vector.
        /// </summary>
        /// <returns>The modified vector.</returns>
        /// <param name="vector">The vector to set the y component of</param>
        /// <param name="height">The y component to set the vector to.</param>
        public static Vector3 SetY(Vector3 vector, float y) {
            Vector3 newVector = vector;
            newVector.y = y;
            return newVector;
        }

        /// <summary>
        /// Scales the x and z components of a vector.
        /// </summary>
        /// <returns>The scaled vector.</returns>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The scale to modify the vector with.</param>
        public static Vector3 ScaleXZ(Vector3 vector, float scale) {
            Vector3 newVector = vector;
            newVector.x += scale;
            newVector.z += scale;
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

        /// <summary>
        /// Gets the string form of a vector with higher precision than Vector3.ToString().
        /// </summary>
        /// <returns>The string form of the given vector.</returns>
        /// <param name="vector">The vector to get a string for.</param>
        public static string GetPreciseString(Vector3 vector) {
            return "(" + vector.x + "," + vector.y + "," + vector.z + ")";
        }
    }
}

