using UnityEngine;
using UnityEngine.UI;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.UI {
    /// <summary>
    /// Handles pausing the game.
    /// </summary>
    class PauseHandler : MonoBehaviour {

        /// <summary> The singleton pause handler instance. </summary>
        private static PauseHandler pauseHandler;
        /// <summary> The singleton pause handler instance. </summary>
        public static PauseHandler instance {
            get { return pauseHandler; }
        }

        /// <summary> Whether the game is paused. </summary>
        private bool _paused;
        /// <summary> Whether the game is paused. </summary>
        public bool paused {
            get {
                return _paused;
            }
            private set {
                _paused = value;
                Cursor.visible = value;
                if (!GameManager.instance.pictureMode) {
                    gamePanel.SetActive(!_paused);
                }
                pausePanel.SetActive(_paused);
                if (_paused) {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                } else {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }

        /// <summary> The pause menu. </summary>
        [SerializeField]
        [Tooltip("The pause menu.")]
        private GameObject pausePanel;
        /// <summary> The in-game UI. </summary>
        [SerializeField]
        [Tooltip("The in-game UI.")]
        private GameObject gamePanel;

        /// <summary>
        /// Initializes the singleton pause handler instance.
        /// </summary>
        private void Awake() {
            pauseHandler = this;
        }

        /// <summary>
        /// Sets up the UI.
        /// </summary>
        private void Start() {
            paused = false;
        }

        /// <summary>
        /// Checks for the pause hotkey.
        /// </summary>
        private void Update() {
            if (InputUtil.GetKeyDown(KeyCode.P)) {
                paused = !paused;
            }
        }

        /// <summary>
        /// Unpause the game.
        /// </summary>
        public void Unpause() {
            paused = false;
        }
    }
}