using UnityEngine;
using System.Collections.Generic;
using Lemmings.Entities;
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

        /// <summary> The resettable objects in the scene. </summary>
        [HideInInspector]
        public List<ResettableObject> resetObjects = new List<ResettableObject>();

        /// <summary> The total number of lemmings in the level. </summary>
        [HideInInspector]
        public int numLemmings;
        /// <summary> The number of active lemmings in the scene. </summary>
        [HideInInspector]
        public int activeLemmings;
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

        /// <summary> The lemming spawner in the scene. </summary>
        [HideInInspector]
        public LemmingSpawner lemmingSpawner;

        /// <summary>
        /// Sets the singleton instance of the game manager.
        /// </summary>
        private void Awake() {
            _instance = this;
        }

        /// <summary>
        /// Checks for the user resetting the level.
        /// </summary>
        private void Update() {
            _currentTime += Time.deltaTime;
            if (activeLemmings <= 0 && lemmingSpawner.IsFinished() ||
                InputUtil.GetKeyDown(KeyCode.R) && !PauseHandler.instance.paused) {
                ResetLevel();
            }
        }

        /// <summary>
        /// Resets the level.
        /// </summary>
        private void ResetLevel() {
            activeLemmings = 0;
            goalLemmings = 0;
            foreach (ResettableObject resetObject in resetObjects) {
                resetObject.Reset();
            }
            _currentTime = 0;
            BlockManager.instance.Reset();
            SurfaceManager.instance.Reset();
        }
    }
}