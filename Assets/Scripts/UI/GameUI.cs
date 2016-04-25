using UnityEngine;
using Lemmings.Util;

namespace Lemmings.UI {
    /// <summary>
    /// Handles the UI in-game.
    /// </summary>
    class GameUI : MonoBehaviour {
        /// <summary> Whether the game is paused. </summary>
        private bool _paused;
        /// <summary> Whether the game is paused. </summary>
        private bool paused {
            get {
                return _paused;
            }
            set {
                _paused = value;
                Cursor.visible = value;
                gamePanel.SetActive(!_paused);
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
        private GameObject pausePanel;

        /// <summary> The in-game UI. </summary>
        private GameObject gamePanel;

        /// <summary>
        /// Sets up the UI.
        /// </summary>
        private void Start() {
            gamePanel = transform.FindChild("Game Panel").gameObject;
            pausePanel = transform.FindChild("Pause Panel").gameObject;
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