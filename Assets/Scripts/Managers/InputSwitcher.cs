using UnityEngine;
using Lemmings.Entities.Player;
using Lemmings.Graphics;

namespace Lemmings.Managers {

    /// <summary>
    /// Switches the scene's input mode.
    /// </summary>
    class InputSwitcher : MonoBehaviour {

        /// <summary> Whether a HoloLens is currently in use. </summary>
        [SerializeField]
        [Tooltip("Whether a HoloLens is currently in use.")]
        private bool hololensEnabled;

        /// <summary> Objects that are enabled in desktop mode only. </summary>
        [SerializeField]
        [Tooltip("Objects that are enabled in desktop mode only.")]
        private GameObject[] desktopObjects;
        /// <summary> Objects that are enabled in HoloLens mode only. </summary>
        [SerializeField]
        [Tooltip("Objects that are enabled in HoloLens mode only.")]
        private GameObject[] hololensObjects;

        /// <summary>
        /// Switches necessary objects to HoloLens mode.
        /// </summary>
        private void Start() {
            SwitchObjects();
        }

        /// <summary>
        /// Switches objects between VR and desktop modes.
        /// </summary>
        private void SwitchObjects() {
            foreach (GameObject desktopObject in desktopObjects) {
                desktopObject.SetActive(!hololensEnabled);
            }
            foreach (GameObject hololensObject in hololensObjects) {
                hololensObject.SetActive(hololensEnabled);
            }
        }
    }
}