using UnityEngine;
using System.Collections.Generic;

namespace Lemmings.Util {
    /// <summary>
    /// Utility methods for querying input.
    /// </summary>
    class InputUtil : MonoBehaviour {

        /// <summary>
        /// Checks if any of the given keys are pressed.
        /// </summary>
        /// <returns>Whether any of the given keys are pressed.</returns>
        /// <param name="keys">The keys to check for being pressed.</param>
        public static bool GetKey(params KeyCode[] keys) {
            foreach (KeyCode key in keys) {
                if (Input.GetKey(key)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if any of the given keys have just been pressed.
        /// </summary>
        /// <returns>Whether any of the given keys have just been pressed.</returns>
        /// <param name="keys">The keys to check for having just been pressed.</param>
        public static bool GetKeyDown(params KeyCode[] keys) {
            foreach (KeyCode key in keys) {
                if (Input.GetKeyDown(key)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the movement of the mouse in the current frame.
        /// </summary>
        /// <returns>The movement of the mouse in the current frame.</returns>
        public static Vector2 GetMouseMovement() {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        /// <summary>
        /// Checks if the left mouse button was pressed.
        /// </summary>
        /// <returns>Whether the left mouse button was pressed.</returns>
        public static bool GetLeftMouseDown() {
            return Input.GetMouseButtonDown(0);
        }

        /// <summary>
        /// Checks if the right mouse button was pressed.
        /// </summary>
        /// <returns>Whether the right mouse button was pressed.</returns>
        public static bool GetRightMouseDown() {
            return Input.GetMouseButtonDown(1);
        }
    }
}