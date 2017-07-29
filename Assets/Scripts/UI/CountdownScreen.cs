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

        /// <summary> Text substituted for the appropriate key combination for starting a level. </summary>
        public const string START_KEY = "#START_KEY";

        /// <summary> The amount of time to count down for. </summary>
        [SerializeField]
        [Tooltip("The amount of time to count down for.")]
        private int countDownTime;

        /// <summary> The text displaying the countdown. </summary>
        [SerializeField]
        [Tooltip("The text displaying the countdown.")]
        private Text text;

        /// <summary> The text displaying any level loading errors that occur. </summary>
        [SerializeField]
        [Tooltip("The text displaying any level loading errors that occur.")]
        private Text errorText;

        /// <summary> The text currently on the screen, with a possible replacement for the starting key combination. </summary>
        private string templateText = "";

        /// <summary> The amount of time remaining in the countdown. </summary>
        private float timeLeft = 1;
        /// <summary> Whether to show the starting key combination prompt. </summary>
        private bool showStart = true;

        /// <summary> Plays countdown decrement sounds. </summary>
        private AudioSource audioSource;
        /// <summary> The sound to play when the countdown ends. </summary>
        [SerializeField]
        [Tooltip("The sound to play when the countdown ends.")]
        private AudioClip countdownEndSound;

        /// <summary>
        /// Sets the singleton instance of the countdown screen.
        /// </summary>
        private void Awake() {
            _instance = this;
        }

        /// <summary>
        /// Does initial text replacement for the starting prompt.
        /// </summary>
        private void Start() {
            InputDetector.OnInputChanged += ChangeController;
            SetText(text.text);
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Sets the message on the countdown screen.
        /// </summary>
        /// <param name="message">The message on the countdown screen.</param>
        public void SetText(string message) {
            templateText = message;
            ChangeController();
        }

        /// <summary>
        /// Sets the error message on the countdown screen.
        /// </summary>
        /// <param name="message">The error message on the countdown screen.</param>
        public void SetError(string error) {
            errorText.text = error;
        }

        /// <summary>
        /// Starts the countdown at the start of the level.
        /// </summary>
        public void StartCountdown() {
            timeLeft = (float) countDownTime;
            gameObject.SetActive(true);
            showStart = false;
            SetError("");
            Update();
        }

        /// <summary>
        /// Updates the countdown.
        /// </summary>
        private void Update() {
            foreach (Text textObject in new Text[]{text, errorText}) {
                textObject.gameObject.SetActive(!IngameMenuHandler.instance.open);
            }
            if (!showStart) {
                int secondsLeft = (int) timeLeft + 1;
                string secondsString = secondsLeft.ToString();
                string prevText = text.text;
                SetText(secondsString);
                Color textColor = text.color;
                textColor.a = timeLeft - (float) secondsLeft + 1;
                text.color = textColor;
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0) {
                    gameObject.GetComponentInParent<AudioSource>().PlayOneShot(countdownEndSound);
                    gameObject.SetActive(false);
                    GameManager.instance.StartLevel();
                } else if (prevText != secondsString) {
                    audioSource.Play();
                }
            }
        }

        /// <summary>
        /// Forces the text on the screen to update for controllers changes.
        /// </summary>
        private void ChangeController() {
            ChangeController(InputDetector.instance.numControllers);
        }

        /// <summary>
        /// Changes the text on the screen when the number of connected controllers changes.
        /// </summary>
        /// <param name="numControllers">The new number of connected controllers.</param>
        private void ChangeController(int numControllers) {
            string replace = numControllers == 0 ? "G" : "PS";
            text.text = templateText.Replace(START_KEY, replace);
        }
    }
}
