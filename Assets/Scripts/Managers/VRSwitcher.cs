using UnityEngine;
using UnityEngine.VR;
using Lemmings.Entities.Player;
using Lemmings.Graphics;

namespace Lemmings.Managers {

    /// <summary>
    /// Switches the scene to VR mode if enabled.
    /// </summary>
    class VRSwitcher : MonoBehaviour {

        /// <summary> Whether an Oculus Rift is currently in use. </summary>
        private bool vrEnabled;

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
            vrEnabled = VRDevice.isPresent;
            SwitchObjects();
        }

        /// <summary>
        /// Checks the status of the Oculus Rift connection.
        /// </summary>
        private void Update() {
            if (VRDevice.isPresent != vrEnabled) {
                vrEnabled = VRDevice.isPresent;
                SwitchObjects();
            }
        }

        /// <summary>
        /// Switches objects between VR and desktop modes.
        /// </summary>
        private void SwitchObjects() {
            foreach (GameObject desktopObject in desktopObjects) {
                desktopObject.SetActive(!vrEnabled);
            }
            foreach (GameObject vrObject in vrObjects) {
                vrObject.SetActive(vrEnabled);
            }
        }
    }
}