using UnityEngine;

namespace Lemmings.Entities.Player {

    /// <summary>
    /// Places the camera on the player.
    /// </summary>
    class PlayerCamera : MonoBehaviour {

        /// <summary>
        /// Makes the camera follow the player.
        /// </summary>
        private void FixedUpdate() {
            //transform.position = PlayerMover.instance.transform.position;
        }
    }
}