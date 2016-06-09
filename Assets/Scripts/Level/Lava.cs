using UnityEngine;
using Lemmings.Entities;

namespace Lemmings.Level {
    /// <summary>
    /// Kills lemmings that touch it.
    /// </summary>
    public class Lava : MonoBehaviour {

        /// <summary>
        /// Kills lemmings that collide.
        /// </summary>
        /// <param name="collision">The collision event that occurred.</param>
        private void OnCollisionEnter(Collision collision) {
            if (collision.collider.tag == "Lemming") {
                collision.collider.GetComponent<Lemming>().Die();
            }
        }
    }
}