using UnityEngine;
using UnityEngine.SceneManagement;
using Lemmings.Managers;

namespace Lemmings.UI {

    /// <summary>
    /// Changes the pause UI based on game state.
    /// </summary>
    class PauseScreen : MonoBehaviour {
        
        /// <summary> Objects to show only before loading a level. </summary>
        [SerializeField]
        [Tooltip("Objects to show only before loading a level.")]
        private GameObject[] objectsBeforeLoad;
        /// <summary> Objects to show only after loading a level. </summary>
        [SerializeField]
        [Tooltip("Objects to show only after loading a level.")]
        private GameObject[] objectsAfterLoad;

        /// <summary>
        /// Disables objects visible after loading.
        /// </summary>
        private void Start() {
            SetAfterLoad(false);
        }

        /// <summary>
        /// Restarts the current level.
        /// </summary>
        public void Restart() {
            GameManager.instance.ResetLevel();
            PauseHandler.instance.Unpause();
        }

        /// <summary>
        /// Switches the pause screen to objects visible after loading.
        /// </summary>
        public void SwitchAfterLoad() {
            SetAfterLoad(true);
            PauseHandler.instance.Unpause();
        }

        /// <summary>
        /// Goes back to the settings menu.
        /// </summary>
        public void ChooseSettings() {
            SceneManager.LoadScene("Settings");
        }

        /// <summary>
        /// Sets the objects that should be visible.
        /// </summary>
        /// <param name="afterLoad">Whether to set the pause screen objects to after loading.</param>
        private void SetAfterLoad(bool afterLoad) {
            foreach (GameObject objectBeforeLoad in objectsBeforeLoad) {
                objectBeforeLoad.SetActive(!afterLoad);
            }
            foreach (GameObject objectAfterLoad in objectsAfterLoad) {
                objectAfterLoad.SetActive(afterLoad);
            }
            enabled = false;
        }
    }
}