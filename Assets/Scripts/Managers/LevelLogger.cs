using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Lemmings.Entities;
using Lemmings.Entities.Player;
using Lemmings.Util;
using Lemmings.Util.Timers;

namespace Lemmings.Managers {
    /// <summary>
    /// Logs player and entity movement.
    /// </summary>
    class LevelLogger : MonoBehaviour {

        /// <summary> The singleton logger instance. </summary>
        private static LevelLogger _instance;
        /// <summary> The singleton logger instance. </summary>
        public static LevelLogger instance {
            get {
                return _instance;
            }
        }

        /// <summary> The game manager in the scene. </summary>
        private GameManager gameManager;
        /// <summary> The log file. </summary>
        private StreamWriter file;
        /// <summary> Timer for periodically logging the game state. </summary>
        private LimitTimerCallback logTimer;

        /// <summary> Whether player logging is enabled. </summary>
        [SerializeField]
        [Tooltip("Whether player logging is enabled.")]
        private bool loggingEnabled;
        /// <summary> The name of the file to write to. </summary>
        [SerializeField]
        [Tooltip("The name of the file to write to.")]
        private string filePath;
        /// <summary> The delay between logged positions. </summary>
        [SerializeField]
        [Tooltip("The delay between logged positions.")]
        private float logDelay;

        /// <summary> Whether logging has stopped occurring. </summary>
        private bool stopped;

        /// <summary> The JSON file that the level is using. </summary>
        internal string json;

        /// <summary> The player in the scene. </summary>
        private PlayerMover player;
        /// <summary> Player positions over time. </summary>
        internal List<Vector3> playerPositions = new List<Vector3>();
        /// <summary> Lemming positions over time. </summary>
        internal List<Vector3>[] lemmingPositions;

        /// <summary>
        /// Sets the logger instance.
        /// </summary>
        private void Awake() {
            _instance = this;
        }

        /// <summary>
        /// Gets the level manager instance.
        /// </summary>
        private void Start() {
            gameManager = GameManager.instance;
            logTimer = new LimitTimerCallback(LogState, logDelay);
            player = PlayerMover.instance;
        }

        /// <summary>
        /// Periodically logs the game state.
        /// </summary>
        private void Update() {
            if (json != null && !stopped) {
                if (lemmingPositions == null) {
                    lemmingPositions = new List<Vector3>[gameManager.numLemmings];
                    for (int i = 0; i < lemmingPositions.Length; i++) {
                        lemmingPositions[i] = new List<Vector3>();
                    }
                }
                logTimer.Run();
            }
        }

        /// <summary>
        /// Writes text to the current log file.
        /// </summary>
        /// <param name="text">The text to log.</param>
        private void WriteToFile() {
            if (loggingEnabled && json != null && !stopped) {
                // Create the log file.
                System.IO.Directory.CreateDirectory(filePath);
                string date = DateTime.Now.ToString().Replace(":", "").Replace("/", "").Replace(" ", "_");
                file = File.CreateText(filePath + "log_" + date + ".txt");

                // Write the JSON data to the file.
                file.WriteLine(json);

                // Write player data to the file.
                file.WriteLine("Player:");
                WriteVectorList(playerPositions);

                // Write lemming data to the file.
                for (int i = 0; i < lemmingPositions.Length; i++) {
                    List<Vector3> lemmingList = lemmingPositions[i];
                    if (lemmingList.Count > 0) {
                        file.WriteLine("Lemming" + i);
                        WriteVectorList(lemmingList);
                    }
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes a list of vectors to the log file.
        /// </summary>
        /// <param name="vectorList">The list of vectors to log.</param>
        private void WriteVectorList(List<Vector3> vectorList) {
            foreach (Vector3 vector in vectorList) {
                file.WriteLine(VectorUtil.GetPreciseString(vector));
            }
        }

        /// <summary>
        /// Logs the current positions of entities in the game.
        /// </summary>
        private void LogState() {
            playerPositions.Add(player.transform.position);

            foreach (Lemming lemming in gameManager.activeLemmings) {
                lemmingPositions[lemming.index].Add(lemming.transform.position);
            }
        }

        /// <summary>
        /// Stops logging from occurring.
        /// </summary>
        internal void Stop() {
            WriteToFile();
            stopped = true;
        }

        /// <summary>
        /// Writes the current game to a file and resets the log.
        /// </summary>
        internal void Reset() {
            WriteToFile();
            stopped = false;
            playerPositions.Clear();
            for (int i = 0; i < lemmingPositions.Length; i++) {
                lemmingPositions[i].Clear();
            }
            logTimer.Reset();
        }

        /// <summary>
        /// Saves the log if the user quits the game.
        /// </summary>
        private void OnApplicationQuit() {
            WriteToFile();
        }
    }
}