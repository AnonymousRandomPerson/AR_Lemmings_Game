using UnityEngine;
using UnityEngine.UI;
using Lemmings.Managers;
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

        /// <summary> The game manager in the scene. </summary>
        private GameManager gameManager;
        /// <summary> The block manager in the scene. </summary>
        private BlockManager blockManager;

        /// <summary> The pause menu. </summary>
        [SerializeField]
        [Tooltip("The pause menu.")]
        private GameObject pausePanel;
        /// <summary> The in-game UI. </summary>
        [SerializeField]
        [Tooltip("The in-game UI.")]
        private GameObject gamePanel;

        /// <summary> Text displaying the successful and total number of lemmings. </summary>
        [SerializeField]
        [Tooltip("Text displaying the successful and total number of lemmings.")]
        private Text lemmingText;
        /// <summary> Text displaying the placed and total number of blocks. </summary>
        [SerializeField]
        [Tooltip("Text displaying the placed and total number of blocks.")]
        private Text blockText;
        /// <summary> Text displaying the elapsed time in the level. </summary>
        [SerializeField]
        [Tooltip("Text displaying the elapsed time in the level.")]
        private Text timerText;

        /// <summary>
        /// Sets up the UI.
        /// </summary>
        private void Start() {
            paused = false;
            gameManager = FindObjectOfType<GameManager>();
            blockManager = gameManager.GetComponent<BlockManager>();
        }

        /// <summary>
        /// Checks for the pause hotkey.
        /// </summary>
        private void Update() {
            if (InputUtil.GetKeyDown(KeyCode.P)) {
                paused = !paused;
            }
            UpdateLemmingText();
            UpdateBlockText();
            UpdateTimer();
        }

        /// <summary>
        /// Unpause the game.
        /// </summary>
        public void Unpause() {
            paused = false;
        }

        /// <summary>
        /// Updates the lemming counter text.
        /// </summary>
        private void UpdateLemmingText() {
            int goalLemmings = gameManager.goalLemmings;
            int numLemmings = gameManager.numLemmings;
            lemmingText.text = goalLemmings + "/" + numLemmings;
            lemmingText.color = goalLemmings == numLemmings ? Color.green : Color.black;
        }

        /// <summary>
        /// Updates the block counter text.
        /// </summary>
        private void UpdateBlockText() {
            int placedBlocks = blockManager.placedBlocks;
            int numBlocks = blockManager.NumBlocks;
            blockText.text = placedBlocks + "/" + numBlocks;
            blockText.color = placedBlocks == numBlocks ? Color.red : Color.black;
        }

        private void UpdateTimer() {
            int currentTime = (int)gameManager.currentTime;
            string seconds = (currentTime % 60).ToString();
            if (currentTime < 10) {
                seconds = "0" + seconds;
            }
            string minutes = (currentTime / 60).ToString();

            timerText.text = minutes + ":" + seconds;
        }
    }
}