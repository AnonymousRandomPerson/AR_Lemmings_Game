using UnityEngine;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.Entities.Blocks {
    /// <summary>
    /// A block that can be placed to steer lemmings.
    /// </summary>
    class DirectionBlock : Block {

        /// <summary> The direction that the block will steer lemmings in. </summary>
        [SerializeField]
        [Tooltip("The direction that the block will steer lemmings in.")]
        private Direction direction;

        /// <summary> The rigidbody on the block. </summary>
        private Rigidbody body;
        /// <summary> The block's renderer. </summary>
        private Renderer blockRenderer;


        /// <summary>
        /// Initializes the block upon creation.
        /// </summary>
        public override void Create() {
            base.Create();
            body = GetComponent<Rigidbody>();
            blockRenderer = GetComponent<Renderer>();
        }

        /// <summary>
        /// Initializes the block upon being placed.
        /// </summary>
        public override void Init() {
            base.Init();
            body.velocity = Vector3.zero;
        }

        /// <summary>
        /// Inflicts the block's effects on a lemming.
        /// </summary>
        /// <param name="lemming">The lemming who triggered the block.</param>
        /// <param name="hit">Collision information.</param>
        public override void AffectLemming(Lemming lemming, RaycastHit hit = new RaycastHit()) {
            Vector3 newDirection;
            if (direction == Direction.Left) {
                newDirection = new Vector3(hit.normal.z, 0, -hit.normal.x);
            } else {
                newDirection = new Vector3(-hit.normal.z, 0, hit.normal.x);
            }
            lemming.transform.forward = newDirection;
        }
    }
}