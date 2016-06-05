using UnityEngine;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.Entities.Blocks {
    /// <summary>
    /// An object that can be placed by the player.
    /// </summary>
    public abstract class Block : MonoBehaviour {

        /// <summary> The block's type. </summary>
        [HideInInspector]
        public BlockType type;

        /// <summary> The rigidbody on the block. </summary>
        private Rigidbody body;

        /// <summary>
        /// Initializes the block upon creation.
        /// </summary>
        public virtual void Create() {
            body = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Initializes the block upon being placed.
        /// </summary>
        public virtual void Init() {
            body.velocity = Vector3.zero;
        }

        /// <summary>
        /// Checks if the block should be despawned from falling out of bounds.
        /// </summary>
        protected virtual void Update() {
            if (transform.position.y < PhysicsUtil.DEATHHEIGHT) {
                Despawn();
            }
        }

        /// <summary>
        /// Affects lemmings that collide with the block.
        /// </summary>
        /// <param name="collider">The object that collided with the block.</param>
        private void OnTriggerEnter(Collider collider) {
            if (collider.tag == "Lemming") {
                AffectLemming(collider.GetComponent<Lemming>());
            }
        }

        /// <summary>
        /// Inflicts the block's effects on a lemming.
        /// </summary>
        /// <param name="lemming">The lemming who triggered the block.</param>
        /// <param name="hit">Collision information.</param>
        public virtual void AffectLemming(Lemming lemming, RaycastHit hit = new RaycastHit()) {
        }

        /// <summary>
        /// Despawns the block.
        /// </summary>
        public void Despawn() {
            BlockManager.instance.DespawnBlock(this);
        }
    }
}

