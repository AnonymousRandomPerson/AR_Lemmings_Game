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
        /// Remove event listeners when the event is destroyed.
        /// </summary>
        private void OnDestroy() {
        	InputDetector.OnInputChanged -= UpdateSelected;
        }

        /// <summary>
        /// Listens to input to enable key selection if no button is selected.
        /// </summary>
        private void Update() {
            StandaloneInputModule module = eventSystem.GetComponent<StandaloneInputModule>();
            if (Mathf.Abs(Input.GetAxis(module.horizontalAxis)) > Mathf.Epsilon || Mathf.Abs(Input.GetAxis(module.verticalAxis)) > Mathf.Epsilon) {
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