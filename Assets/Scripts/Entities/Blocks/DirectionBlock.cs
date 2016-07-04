using UnityEngine;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.Entities.Blocks {
    /// <summary>
    /// A block that can be placed to steer lemmings.
    /// </summary>
    class DirectionBlock : Block {

        /// <summary>
        /// Inflicts the block's effects on a lemming.
        /// </summary>
        /// <param name="lemming">The lemming who triggered the block.</param>
        /// <param name="hit">Collision information.</param>
        public override void AffectLemming(Lemming lemming, RaycastHit hit = new RaycastHit()) {
            Vector3 xz = VectorUtil.SetY(transform.forward, 0);
            xz.Normalize();

            // Turns the lemming around if it would be walking into the block.
            if (VectorUtil.ApproximatelyEqual(lemming.transform.forward, xz)) {
                xz = -xz;
            }

            lemming.transform.forward = xz;
        }
    }
}