using UnityEngine;
using Lemmings.Managers;

namespace Lemmings.UI {
    /// <summary>
    /// The screen that is displayed when generating a level.
    /// </summary>
    class LoadingScreen : MonoBehaviour {

        /// <summary> The panel that displays the loading screen. </summary>
        [SerializeField]
        [Tooltip("The panel that displays the loading screen.")]
        private GameObject loadingPanel;
        /// <summary> The game manager in the scene. </summary>
        private GameManager gameManager;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start() {
            gameManager = GameManager.instance;
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        private void Update() {
            loadingPanel.SetActive(gameManager.isLoading);
        }
    }
}
