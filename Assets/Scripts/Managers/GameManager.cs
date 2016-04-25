using UnityEngine;
using System.Collections.Generic;
using Lemmings.Entities;
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

        /// <summary> The number of active lemmings in the scene. </summary>
        public int numLemmings;

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
            if (numLemmings <= 0 || InputUtil.GetKeyDown(KeyCode.R)) {
                ResetLevel();
            }
        }

        /// <summary>
        /// Resets the level.
        /// </summary>
        private void ResetLevel() {
            numLemmings = 0;
            foreach (ResettableObject resetObject in resetObjects) {
                resetObject.Reset();
            }
            BlockManager.instance.Reset();
        }
    }
}