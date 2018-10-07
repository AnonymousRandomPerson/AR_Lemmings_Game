using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Lemmings.Managers;

namespace Lemmings.UI {
    /// <summary>
    /// Main menu UI event handler.
    /// </summary>
    class MainMenu : MonoBehaviour {

        /// <summary> The level to load when starting the game. </summary>
        [SerializeField]
        [Tooltip("The level to load when starting the game.")]
        private string levelName;

        /// <summary> Slider for the path setting. </summary>
        [SerializeField]
        [Tooltip("Slider for the path setting.")]
        private SliderBase pathSlider;
        /// <summary> Slider for the movement setting. </summary>
        [SerializeField]
        [Tooltip("Slider for the movement setting.")]
        private SliderBase movementSlider;
        /// <summary> Slider for the difficulty setting. </summary>
        [SerializeField]
        [Tooltip("Slider for the difficulty setting.")]
        private SliderBase difficultySlider;
        /// <summary> Input field for setting the server URL. </summary>
        [SerializeField]
        [Tooltip("Input field for setting the server URL.")]
        private InputField serverField;

        /// <summary> The settings instance. </summary>
        Settings settings;

        /// <summary>
        /// Loads the level settings.
        /// </summary>
        private void Start() {
            settings = Settings.instance;
            settings.LoadSettings();
            pathSlider.sliderValue = settings.pathSetting;
            movementSlider.sliderValue = settings.movementSetting;
            difficultySlider.sliderValue = settings.difficultySetting;
            serverField.text = settings.serverURL;
        }

        /// <summary>
        /// Loads the level.
        /// </summary>
        public void LoadLevel() {
            SceneManager.LoadScene(levelName);
        }
    }
}
