using UnityEngine;
using System.Collections.Generic;
using Lemmings.Entities;
using Lemmings.Entities.Player;
using Lemmings.Graphics;
using Lemmings.UI;
using Lemmings.Util;

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

        /// <summary> The number of times all lemmings have died. </summary>
        public static int numDeaths;

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
            isLoading = true;
            pathRenderer = GetComponent<TravelHistoryRenderer>();
        }

        /// <summary>
        /// Checks for the user resetting the level.
        /// </summary>
        private void Update() {
            if (isPlaying) {
                _currentTime += Time.deltaTime;
                if (InputUtil.GetKeyDown(KeyCode.R) && !PauseHandler.instance.paused) {
                    ResetLevel();
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
            isPlaying = true;
        }
    }
}