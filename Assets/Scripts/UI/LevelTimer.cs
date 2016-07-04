using UnityEngine;
using UnityEngine.UI;
using Lemmings.Managers;

namespace Lemmings.UI {
    /// <summary>
    /// Keeps track of the amount of time that has elapsed during the level.
    /// </summary>
    class LevelTimer : MonoBehaviour {

        /// <summary> The text to update. </summary>
        private Text text;

        /// <summary> The game manager to get the current time from. </summary>
        private GameManager gameManager;

        /// <summary>
        /// Finds needed objects in the scene.
        /// </summary>
        private void Start() {
            text = GetComponent<Text>();
            gameManager = GameManager.instance;
        }
        
        /// <summary>
        /// Updates the time text.
        /// </summary>
        private void Update() {
            int currentTime = (int)gameManager.currentTime;
            int seconds = (currentTime % 60);
            string secondsText = seconds.ToString();
            if (seconds < 10) {
                secondsText = "0" + secondsText;
            }
            string minutesText = (currentTime / 60).ToString();

            text.text = minutesText + ":" + secondsText;
        }
    }
}