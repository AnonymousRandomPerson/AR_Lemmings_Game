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
        /// Listens to input to enable key selection if no button is selected.
        /// </summary>
        private void Update() {
            StandaloneInputModule module = eventSystem.GetComponent<StandaloneInputModule>();
            if (Input.GetAxis(module.horizontalAxis) != 0 || Input.GetAxis(module.verticalAxis) != 0) {
                selectFromNone();
            }
        }

        /// <summary>
        /// Selects or deselects the UI when a controller is connected or disconnected.
        /// </summary>
        /// <param name="numControllers">The current number of connected controllers</param>
        private void UpdateSelected(int numControllers) {
            if (numControllers == 0) {
                eventSystem.SetSelectedGameObject(null);
            } else {
                selectFromNone();
            }
        }

        /// <summary>
        /// Selects the default select object if no objects are selected.
        /// </summary>
        private void selectFromNone() {
            if (eventSystem.currentSelectedGameObject == null) {
                eventSystem.SetSelectedGameObject(defaultSelected);
            }
        }
    }
}