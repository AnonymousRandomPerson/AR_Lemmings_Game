using UnityEngine;
using UnityEngine.SceneManagement;
using Lemmings.Managers;

namespace Lemmings.UI {

    /// <summary>
    /// Changes the in-game menu UI based on game state.
    /// </summary>
    class IngameMenuScreen : MonoBehaviour {

        /// <summary> The singleton in-game menu screen instance. </summary>
        private static IngameMenuScreen ingameMenuScreen;
        /// <summary> The singleton in-game menu screen instance. </summary>
        public static IngameMenuScreen instance {
            get { return ingameMenuScreen; }
        }
        
        /// <summary> Objects to show only before loading a level. </summary>
        [SerializeField]
        [Tooltip("Objects to show only before loading a level.")]
        private GameObject[] objectsBeforeLoad;
        /// <summary> Objects to show only after loading a level. </summary>
        [SerializeField]
        [Tooltip("Objects to show only after loading a level.")]
        private GameObject[] objectsAfterLoad;

        /// <summary>
        /// Initializes the singleton in-game menu handler instance.
        /// </summary>
        private void Awake() {
            ingameMenuScreen = this;
        }

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
            IngameMenuHandler.instance.CloseMenu();
        }

        /// <summary>
        /// Switches the in-game menu screen to objects visible after loading.
        /// </summary>
        public void SwitchAfterLoad() {
            SetAfterLoad(true);
            IngameMenuHandler.instance.CloseMenu();
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
        /// <param name="afterLoad">Whether to set the in-game menu screen objects to after loading.</param>
        private void SetAfterLoad(bool afterLoad) {
            foreach (GameObject objectBeforeLoad in objectsBeforeLoad) {
                objectBeforeLoad.SetActive(!afterLoad);
            }
            foreach (GameObject objectAfterLoad in objectsAfterLoad) {
                objectAfterLoad.SetActive(afterLoad);
            }
            enabled = false;
        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        public void Quit() {
            Application.Quit();
        }
    }
}