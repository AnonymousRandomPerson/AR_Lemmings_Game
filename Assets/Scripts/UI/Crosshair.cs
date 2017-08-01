using UnityEngine;
using UnityEngine.UI;
using Lemmings.Entities.Player;
using Lemmings.Enums;
using Lemmings.Managers;

namespace Lemmings.UI {
    /// <summary>
    /// The crosshair used to indicate blocks placement.
    /// </summary>
    class Crosshair : MonoBehaviour {

        /// <summary> The player placing blocks. </summary>
        private PlayerPlacer player;
        /// <summary> The crosshair icon. </summary>
        private Text crosshair;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start() {
            player = PlayerPlacer.instance;
            crosshair = GetComponent<Text>();
        }

        /// <summary>
        /// Update the object.
        /// </summary>
        private void Update() {
            crosshair.enabled = !GameManager.instance.isCountingDown;
            if (crosshair.enabled) {
                PlaceStatus placeStatus = player.GetPlaceStatus();
                Color crosshairColor = Color.black;
                switch (placeStatus) {
                case PlaceStatus.Able: crosshairColor = Color.green; break;
                case PlaceStatus.Out: crosshairColor = Color.red; break;
                case PlaceStatus.Range: crosshairColor = Color.black; break;
                case PlaceStatus.Rotate: crosshairColor = Color.yellow; break;
                }

                crosshair.color = crosshairColor;
            }
        }
    }
}
