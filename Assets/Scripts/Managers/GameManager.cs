using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Lemmings.Entities;
using Lemmings.Entities.Player;
using Lemmings.Graphics;
using Lemmings.UI;
using Lemmings.Util;
using System.Net;

namespace Lemmings.Managers {

    /// <summary>
    /// Handles general game operations.
    /// </summary>
    class GameManager : MonoBehaviour {

        /// <summary> The singleton instance of the game manager. </summary>
        private static GameManager _instance;
        /// <summary> The singleton instance of the game manager. </summary>
        public static GameManager instance {
            get {
                return _instance;
            }
        }

        /// <summary> The renderer for path histories. </summary>
        private TravelHistoryRenderer pathRenderer;

        /// <summary> The resettable objects in the scene. </summary>
        [HideInInspector]
        public List<ResettableObject> resetObjects = new List<ResettableObject>();

        /// <summary> The total number of lemmings in the level. </summary>
        [HideInInspector]
        public int numLemmings;
        /// <summary> The active lemmings in the scene. </summary>
        internal List<Lemming> activeLemmings = new List<Lemming>();
        /// <summary> The number of lemmings that have reached the goal. </summary>
        [HideInInspector]
        public int goalLemmings;

        /// <summary> The amount of time that has elapsed in the level. </summary>
        private float _currentTime;
        /// <summary> The amount of time that has elapsed in the level. </summary>
        public float currentTime {
            get { return _currentTime; }
        }

        /// <summary> Whether to freeze objects in place. </summary>
        [SerializeField]
        [Tooltip("Whether to freeze objects in place.")]
        private bool PictureMode;
        /// <summary> Whether to freeze objects in place. </summary>
        public bool pictureMode {
            get { return PictureMode; }
        }

        /// <summary> Whether to hide the in-game UI. </summary>
        [SerializeField]
        [Tooltip("Whether to hide the in-game UI.")]
        private bool DisableUI;
        /// <summary> Whether to hide the in-game UI. </summary>
        public bool disableUI {
            get { return DisableUI; }
        }

        /// <summary> Whether to show path history after the level ends. </summary>
        [SerializeField]
        [Tooltip("Whether to show path history after the level ends.")]
        private bool showPath;

        /// <summary> The lemming spawner in the scene. </summary>
        [HideInInspector]
        public LemmingSpawner lemmingSpawner;

        /// <summary> Whether the level is currently being played.</summary>
        internal bool isPlaying;
        /// <summary> Whether the level is loading. </summary>
        internal bool isLoading;
        /// <summary> Whether the start countdown is occurring. </summary>
        private bool _isCountingDown;
        /// <summary> Whether the start countdown is occurring. </summary>
        public bool isCountingDown {
            get { return _isCountingDown; }
        }

        /// <summary> The number of times all lemmings have died. </summary>
        public static int numDeaths;

        /// <summary> Whether to freeze lemming movement. </summary>
        private bool _freezeLemmings;
        /// <summary> Whether to freeze lemming movement. </summary>
        public bool freezeLemmings {
            get { return _freezeLemmings; }
        }

        /// <summary> Whether a level has been requested from the service. </summary>
        internal bool _levelRequested;
        /// <summary> Whether a level has been requested from the service. </summary>
        public bool LevelRequested {
            get { return _levelRequested; }
        }

        /// <summary> Whether the game UI should be hidden outside of pause mode. </summary>
        public bool HideGameUI {
            get { return pictureMode || disableUI || !_levelRequested; }
        }

        /// <summary>
        /// Sets the singleton instance of the game manager.
        /// </summary>
        private void Awake() {
            _instance = this;
        }

        /// <summary>
        /// Initializes manager references.
        /// </summary>
        private void Start() {
            pathRenderer = GetComponent<TravelHistoryRenderer>();
            PlayerPlacer.instance.enabled = false;
        }

        /// <summary>
        /// Checks for the user resetting the level.
        /// </summary>
        private void Update() {
            if (!_levelRequested && InputUtil.GetButtons(InputCode.NewLevel, InputCode.RerollLevel)) {
                GetComponent<LevelCreator>().RequestLevel();
                CountdownScreen.instance.SetText("");
                CountdownScreen.instance.SetError("");
            }

            if (isPlaying && !PauseHandler.instance.paused) {
                if (!_isCountingDown) {
                    _currentTime += Time.deltaTime;
                }

                if (InputUtil.GetButtonDown(InputCode.ResetLevel)) {
                    if (InputUtil.GetButton(InputCode.RerollLevel)) {
                        RestartScene();
                    } else {
                        ResetLevel();
                    }
                } else if (CountLemmings() == 0 &&
                    lemmingSpawner != null &&
                    lemmingSpawner.IsFinished() &&
                    !pathRenderer.visible) {
                    numDeaths++;
                    if (showPath) {
                        pathRenderer.visible = true;
                        PlayerMover.instance.noClip = true;
                        isPlaying = false;
                    } else {
                        ResetLevel();
                    }
                }

                if (InputUtil.GetButtonDown(InputCode.FreezeLemmings)) {
                    _freezeLemmings = !freezeLemmings;
                }
            }
        }

        /// <summary>
        /// Adds a lemming to the tracking list.
        /// </summary>
        /// <param name="lemming">The lemming to add.</param>
        public void AddLemming(Lemming lemming) {
            activeLemmings.Add(lemming);
        }

        /// <summary>
        /// Removes a lemming from the tracking list.
        /// </summary>
        /// <param name="lemming">The lemming to remove.</param>
        public void RemoveLemming(Lemming lemming) {
            activeLemmings.Remove(lemming);
        }

        /// <summary>
        /// Counts the number of active lemmings.
        /// </summary>
        /// <returns>The number of active lemmings.</returns>
        public int CountLemmings() {
            return activeLemmings.Count;
        }

        /// <summary>
        /// Resets the level.
        /// </summary>
        public void ResetLevel() {
            activeLemmings.Clear();
            goalLemmings = 0;
            foreach (ResettableObject resetObject in resetObjects) {
                resetObject.Reset();
            }
            _currentTime = 0;
            BlockManager.instance.Reset();
            SurfaceManager.instance.Reset();
            LevelLogger.instance.Reset();
            pathRenderer.Reset();

            CountDownStart();
        }

        /// <summary>
        /// Restarts the entire scene, requiring a level to be generated again.
        /// </summary>
        public void RestartScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Starts the countdown for the level start.
        /// </summary>
        internal void CountDownStart() {
            _isCountingDown = true;

            CountdownScreen.instance.StartCountdown();
            PlayerPlacer.instance.enabled = true;
            isPlaying = true;
            GetComponent<Visibility>().ApplySetting();

            PauseHandler.instance.SetGamePanelVisibility(false);
        }

        /// <summary>
        /// Starts spawning lemmings after the countdown ends.
        /// </summary>
        public void StartLevel() {
            _isCountingDown = false;
            PauseHandler.instance.SetGamePanelVisibility(true);
        }

        /// <summary>
        /// Invokes error handling when level loading from the server fails.
        /// </summary>
        /// <param name="errorMessage">The error message produced when the error occurred.</param>
        public void FailLevelLoad(string errorMessage) {
            isLoading = false;
            _levelRequested = false;
            CountdownScreen.instance.SetText("Error loading level; retry with " + CountdownScreen.START_KEY + ".");
            CountdownScreen.instance.SetError(errorMessage);
        }
    }
}