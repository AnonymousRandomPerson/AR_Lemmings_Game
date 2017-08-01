using UnityEngine;

namespace Lemmings.Managers {

    /// <summary>
    /// Keeps track of connected external input devices like controllers.
    /// </summary>
    class InputDetector : MonoBehaviour {

        /// <summary> The current number of connected controllers. </summary>
        public int numControllers {
            get;
            private set;
        }

        /// <summary> Reacts to the number of controllers changing. </summary>
        public delegate void ChangeControllers(int numControllers);
        /// <summary> Called whenever the number of connected controllers changes. </summary>
        public static event ChangeControllers OnInputChanged;

        /// <summary> The singleton instance of the object. </summary>
        public static InputDetector instance {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the object.
        /// </summary>
        private void Awake() {
            instance = this;
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        private void Update() {
            string[] controllers = Input.GetJoystickNames();
            if (controllers.Length != numControllers) {
                numControllers = controllers.Length;
                OnInputChanged(numControllers);
            }
        }
    }
}