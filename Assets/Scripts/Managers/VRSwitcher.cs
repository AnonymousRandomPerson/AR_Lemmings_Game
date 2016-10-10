using UnityEngine;

namespace Lemmings.Managers {

    /// <summary>
    /// Switches the scene to VR mode if enabled.
    /// </summary>
    class VRSwitcher : MonoBehaviour {

        /// <summary> Whether to enable VR objects. </summary>
        [SerializeField]
        [Tooltip("Whether to enable VR objects.")]
        internal bool vrEnabled;

        /// <summary> Objects that are enabled in desktop mode only. </summary>
        [SerializeField]
        [Tooltip("Objects that are enabled in desktop mode only.")]
        private GameObject[] desktopObjects;
        /// <summary> Objects that are enabled in VR mode only. </summary>
        [SerializeField]
        [Tooltip("Objects that are enabled in VR mode only.")]
        private GameObject[] vrObjects;

        /// <summary>
        /// Switches necessary objects to VR mode.
        /// </summary>
        private void Start() {
            foreach (GameObject desktopObject in desktopObjects) {
                desktopObject.SetActive(!vrEnabled);
            }
            foreach (GameObject vrObject in vrObjects) {
                vrObject.SetActive(vrEnabled);
            }
        }
    }
}