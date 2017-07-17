using UnityEngine;
using UnityEngine.UI;

using Lemmings.Managers;

namespace Lemmings.UI {
    /// <summary>
    /// Counts down before the start of the level.
    /// </summary>
    class CountdownScreen : MonoBehaviour {

        /// <summary> The singleton instance of the countdown screen. </summary>
        private static CountdownScreen _instance;
        /// <summary> The singleton instance of the countdown screen. </summary>
        public static CountdownScreen instance {
            get {
                return _instance;
            }
        }

        /// <summary> The amount of time to count down for. </summary>
        [SerializeField]
        [Tooltip("The amount of time to count down for.")]
        private int countDownTime;

        /// <summary> The text displaying the countdown. </summary>
        [SerializeField]
        [Tooltip("The text displaying the countdown.")]
        private Text text;

        /// <summary> The amount of time remaining in the countdown. </summary>
        private float timeLeft = 1;
        /// <summary> Whether to show the starting key combination prompt. </summary>
        private bool showStart = true;

        /// <summary>
        /// Sets the singleton instance of the countdown screen.
        /// </summary>
        private void Awake() {
            _instance = this;
        }

        /// <summary>
        /// Starts the countdown at the start of the level.
        /// </summary>
        public void StartCountdown() {
            timeLeft = (float) countDownTime;
            gameObject.SetActive(true);
            showStart = false;
            Update();
        }

        /// <summary>
        /// Updates the countdown.
        /// </summary>
        private void Update() {
            if (!showStart) {
                int secondsLeft = (int) timeLeft + 1;
                text.text = secondsLeft.ToString();
                Color textColor = text.color;
                textColor.a = timeLeft - (float) secondsLeft + 1;
                text.color = textColor;
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0) {
                    gameObject.SetActive(false);
                    GameManager.instance.StartLevel();
                }
            }
        }
    }
}
