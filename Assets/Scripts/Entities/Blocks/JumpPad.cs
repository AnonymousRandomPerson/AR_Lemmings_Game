using UnityEngine;

namespace Lemmings.Entities.Blocks {
    /// <summary>
    /// A pad that propels lemmings upwards in the direction it faces.
    /// </summary>
    class JumpPad : Block {

        /// <summary> The amount of force that the jump pad exerts on lemmings. </summary>
        [SerializeField]
        [Tooltip("The amount of force that the jump pad exerts on lemmings.")]
        private int force;

        /// <summary>
        /// Inflicts the block's effects on a lemming.
        /// </summary>
        /// <param name="lemming">The lemming who triggered the block.</param>
        /// <param name="hit">Collision information.</param>
        public override void AffectLemming(Lemming lemming, RaycastHit hit = new RaycastHit()) {
            if (lemming.CanJump()) {
                lemming.body.AddForce((transform.forward + Vector3.up) * force);
            }
        }
    }
}