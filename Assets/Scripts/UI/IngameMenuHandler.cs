using UnityEngine;
using UnityEngine.UI;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.UI {
    /// <summary>
    /// Handles display of the in-game menu.
    /// </summary>
    class IngameMenuHandler : MonoBehaviour {

        /// <summary> The singleton in-game menu handler instance. </summary>
        private static IngameMenuHandler ingameMenuHandler;
        /// <summary> The singleton in-game menu handler instance. </summary>
        public static IngameMenuHandler instance {
            get { return ingameMenuHandler; }
        }

        /// <summary> Whether the menu is open. </summary>
        private bool _open;
        /// <summary> Whether the menu is open. </summary>
        public bool open {
            get {
                return _open;
            }
            internal set {
                _open = value;
                Cursor.visible = value;
                SetGamePanelVisibility(!_open);
                ingameMenuPanel.SetActive(_open);
                if (_open) {
                    Cursor.lockState = CursorLockMode.None;
                } else {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }

        /// <summary> The in-game menu. </summary>
        [SerializeField]
        [Tooltip("The in-game menu.")]
        private GameObject ingameMenuPanel;
        /// <summary> The in-game UI. </summary>
        [SerializeField]
        [Tooltip("The in-game UI.")]
        private GameObject gamePanel;

        /// <summary>
        /// Initializes the singleton in-game menu handler instance.
        /// </summary>
        private void Awake() {
            ingameMenuHandler = this;
        }

        /// <summary>
        /// Sets up the UI.
        /// </summary>
        private void Start() {
            open = false;
        }

        /// <summary>
        /// Checks for the in-game menu hotkey.
        /// </summary>
        private void Update() {
            if (!GameManager.instance.isCountingDown && !GameManager.instance.isLoading && InputUtil.GetButtonDown(InputCode.MenuToggle)) {
                open = !open;
                GetComponentInParent<AudioSource>().Play();
            }
        }

        /// <summary>
        /// Closes the menu.
        /// </summary>
        public void CloseMenu() {
            open = false;
        }

        /// <summary>
        /// Sets whether the game panel is visible.
        /// </summary>
        /// <param name="visible">Whether the game panel is visible.</param>
        public void SetGamePanelVisibility(bool visible) {
            if (GameManager.instance.HideGameUI) {
                gamePanel.SetActive(false);
            } else if (GameManager.instance.isPlaying) {
                gamePanel.SetActive(visible);
            }
        }
    }
}