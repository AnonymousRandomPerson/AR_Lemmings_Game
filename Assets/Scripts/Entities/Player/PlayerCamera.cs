using UnityEngine;

namespace Lemmings.Entities.Player {

    /// <summary>
    /// Places the camera on the player.
    /// </summary>
    class PlayerCamera : MonoBehaviour {

        /// <summary>
        /// Makes the UI follow the player.
        /// </summary>
        private void Update() {
            //Debug.Log(transform.FindChild("Game UI VR").localPosition);
            //transform.FindChild("Game UI VR").localPosition = Vector3.zero;
        }
    }
}