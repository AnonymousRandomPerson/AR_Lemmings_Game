using UnityEngine;

namespace Lemmings.Level {
    /// <summary>
    /// A pad that propels lemmings upwards in the direction it faces.
    /// </summary>
    class JumpPad : MonoBehaviour {

        /// <summary>
        /// Propels lemmings that step over the pad.
        /// </summary>
        /// <param name="collider">The collider that hit the pad.</param>
        private void OnTriggerEnter(Collider collider) {
            if (collider.tag == "Lemming") {
                Debug.DrawRay(transform.position, (transform.forward + Vector3.up) * 100, Color.red, 1);
                collider.attachedRigidbody.AddForce((transform.forward + Vector3.up) * 100);
            }
        }
    }
}