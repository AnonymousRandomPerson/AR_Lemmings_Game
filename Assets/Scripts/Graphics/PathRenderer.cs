using UnityEngine;
using System.Collections.Generic;
using Lemmings.Entities;
using Lemmings.Entities.Player;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.Graphics {
    /// <summary>
    /// Renders paths following entity movement.
    /// </summary>
    class PathRenderer : MonoBehaviour {

        /// <summary> The logger in the scene. </summary>
        private LevelLogger logger;

        /// <summary> The game object that holds the path lines. </summary>
        private GameObject pathContainer;

        /// <summary> Log file to display paths from. </summary>
        [SerializeField]
        [Tooltip("Log file to display paths from.")]
        private TextAsset logFile;

        /// <summary> Whether path lines are visible. </summary>
        [Tooltip("Whether path lines are visible.")]
        public bool visible;
        /// <summary> Whether lines have been drawn. </summary>
        private bool drawn;

        /// <summary> The height offset to the player path. </summary>
        private float playerHeightOffset;

        /// <summary> The width of the paths. </summary>
        [SerializeField]
        [Tooltip("The width of the paths.")]
        private float pathWidth;

        /// <summary> The material used to render the path. </summary>
        private Material pathMaterial;
        /// <summary> The color of the player path. </summary>
        [SerializeField]
        [Tooltip("The color of the player path.")]
        private Color playerPathColor;
        /// <summary> The color of the lemming paths. </summary>
        [SerializeField]
        [Tooltip("The color of the lemming paths.")]
        private Color lemmingPathColor;

        /// <summary> The minimum distance before drawing a path line. </summary>
        [SerializeField]
        [Tooltip("The minimum distance before drawing a path line.")]
        private float distanceThreshold;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start() {
            logger = LevelLogger.instance;
            pathContainer = ObjectUtil.CreateNewObject("Paths");
            pathMaterial = new Material(Shader.Find("Particles/Additive"));

            if (logFile != null) {
                visible = true;
            }
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        private void Update() {
            if (visible) {
                if (!drawn) {
                    DrawLines();
                }
            } else if (drawn) {
                RemoveLines();
            }
        }

        /// <summary>
        /// Draws lines representing entity paths.
        /// </summary>
        private void DrawLines() {
            if (drawn) {
                RemoveLines();
            }
            drawn = true;

            List<Vector3> playerPositions;
            List<Vector3>[] lemmingPositions;
            if (logFile == null) {
                playerPositions = logger.playerPositions;
                lemmingPositions = logger.lemmingPositions;
            } else {
                string logText = logFile.text;

                int currentIndex = InputUtil.GetIndexAfter(logText, "Player:\n");
                playerPositions = InputUtil.ParseVectorList(logText, ref currentIndex);

                List<List<Vector3>> lemmingPositionsList = new List<List<Vector3>>();
                while (currentIndex < logText.Length && logText[currentIndex] == 'L') {
                    currentIndex = logText.IndexOf('(', currentIndex);
                    lemmingPositionsList.Add(InputUtil.ParseVectorList(logText, ref currentIndex));
                }

                int numLemmings = lemmingPositionsList.Count;
                lemmingPositions = new List<Vector3>[numLemmings];
                for (int i = 0; i < numLemmings; i++) {
                    lemmingPositions[i] = lemmingPositionsList[i];
                }
            }

            DrawLine(playerPositions, playerPathColor, true);
            if (lemmingPositions != null) {
                foreach (List<Vector3> lemmingPath in lemmingPositions) {
                    DrawLine(lemmingPath, lemmingPathColor);
                }
            }
        }

        /// <summary>
        /// Draws a line representing an entity's path.
        /// </summary>
        /// <param name="path">The path to draw.</param>
        /// <param name="color">The color of the path to draw.</param>
        private void DrawLine(List<Vector3> path, Color color, bool isPlayer = false) {
            if (path.Count > 0) {
                Vector3 lastPosition = path[0];
                for (int i = 1; i < path.Count; i++) {
                    if (Vector3.Distance(lastPosition, path[i]) >= distanceThreshold) {
                        GameObject lineObject = ObjectUtil.CreateNewObject("Path", pathContainer);
                        LineRenderer line = lineObject.AddComponent<LineRenderer>();
                        Vector3[] linePositions = new Vector3[]{lastPosition, path[i]};
                        for (int j = 0; j < linePositions.Length; j++) {
                            float heightOffset;
                            if (isPlayer) {
                                if (playerHeightOffset == 0) {
                                    playerHeightOffset = PlayerMover.instance.height;
                                }
                                heightOffset = playerHeightOffset;
                            } else {
                                heightOffset = Lemming.height;
                            }
                            linePositions[j] += Vector3.down * (heightOffset / 2 - pathWidth * 2);
                            line.SetPosition(j, linePositions[j]);
                        }
                        line.material = pathMaterial;
                        line.SetColors(color, color);
                        line.SetWidth(pathWidth, pathWidth);

                        lastPosition = path[i];
                    }
                }
            }
        }

        /// <summary>
        /// Removes the drawn lines.
        /// </summary>
        private void RemoveLines() {
            drawn = false;
            foreach (Transform line in pathContainer.transform) {
                Destroy(line.gameObject);
            }
        }

        /// <summary>
        /// Resets the drawn lines.
        /// </summary>
        public void Reset() {
            visible = false;
            RemoveLines();
        }
    }
}
