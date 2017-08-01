using UnityEngine;
using UnityEngine.UI;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.UI {
    /// <summary>
    /// Handles display of the in-game menu.
    /// </summary>
    class IngameMenuHandler : MonoBehaviour {

        /// <summary> Called when the in-game menu is opened or closed. </summary>
        public delegate void ToggleIngameMenu(bool open);
        /// <summary> Called when the in-game menu is opened or closed. </summary>
        public static event ToggleIngameMenu OnIngameMenuChanged;

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
                SetGamePanelVisibility(!_open);
                ingameMenuPanel.SetActive(_open);
                Cursor.visible = value;
                Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
                OnIngameMenuChanged(value);
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

        /// <summary> The sound to play when displaying the menu. </summary>
        [SerializeField]
        [Tooltip("The sound to play when displaying the menu.")]
        private AudioClip displayMenuSound;

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
                AudioSource audioSource = GetComponentInParent<AudioSource>();
                if (open) {
                    audioSource.PlayOneShot(displayMenuSound);
                } else {
                    audioSource.Play();
                }
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