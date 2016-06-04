using UnityEngine;
using System.Collections.Generic;
using System.Net;
using Lemmings.Entities;
using Lemmings.Level;

namespace Lemmings.Managers {
    /// <summary>
    /// Creates a ribbon path from input.
    /// </summary>
    class LevelCreator : MonoBehaviour {
        
        /// <summary> Goal resource to be instantiated from. </summary>
        [SerializeField]
        [Tooltip("Goal resource to be instantiated from.")]
        private Goal goalPrefab;
        /// <summary> Texture for virtual platforms. </summary>
        [SerializeField]
        [Tooltip("Texture for virtual platforms.")]
        private Material virtualPlatformMaterial;

        /// <summary> Lemming resource to be instantiated from. </summary>
        [SerializeField]
        [Tooltip("Lemming resource to be instantiated from.")]
        private Lemming lemmingPrefab;

        /// <summary> JSON file to load the level from. </summary>
        [SerializeField]
        [Tooltip("JSON file to load the level from.")]
        private TextAsset json;

        /// <summary> The height of virtual platforms. </summary>
        [SerializeField]
        [Tooltip("The height of virtual platforms.")]
        private float platformHeight;

        /// <summary>
        /// Creates the level from either the level AI server or a local JSON.
        /// </summary>
        private void Start() {
            if (json == null) {
                // Connect to the server to get JSON file.
                NetworkingManager.instance.ProcessStringFromURL((jsonText) =>
                    {
                        CreateLevel(jsonText);
                    });
            } else {
                // Hard-coded JSON resource for testing.
                CreateLevel(json.text);
            }
        }

        /// <summary>
        /// Creates the level.
        /// </summary>
        /// <param name="jsonText">The JSON text to create the level with.</param>
        private void CreateLevel(string jsonText) {
        }
    }
}