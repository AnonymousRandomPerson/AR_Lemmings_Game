using UnityEngine;
using UnityEngine.EventSystems;
using Lemmings.Managers;

namespace Lemmings.UI {

    /// <summary>
    /// 
    /// </summary>
    class ControllerSelector : MonoBehaviour {

        /// <summary> The event system that listens to input. </summary>
        private EventSystem eventSystem;
        /// <summary> The default game object to be selected. </summary>
        private GameObject defaultSelected;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start() {
            InputDetector.OnInputChanged += UpdateSelected;
            eventSystem = FindObjectOfType<EventSystem>();
            defaultSelected = eventSystem.firstSelectedGameObject;
            eventSystem.firstSelectedGameObject = null;
        }

        /// <summary>
        /// Selects or deselects the UI when a controller is connected or disconnected.
        /// </summary>
        /// <param name="numControllers">The current number of connected controllers</param>
        private void UpdateSelected(int numControllers) {
            if (numControllers == 0) {
                eventSystem.SetSelectedGameObject(null);
            } else if (eventSystem.currentSelectedGameObject == null) {
                eventSystem.SetSelectedGameObject(defaultSelected);
            }
        }
    }
}