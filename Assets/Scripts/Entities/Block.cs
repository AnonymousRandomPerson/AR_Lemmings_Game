using UnityEngine;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.Entities {
    /// <summary>
    /// A block that can be placed to block Lemmings.
    /// </summary>
    class Block : MonoBehaviour {

        /// <summary> The rigidbody on the block. </summary>
        private Rigidbody _body;
        /// <summary> The rigidbody on the block. </summary>
        public Rigidbody body {
            get {
                return _body;
            }
        }

        /// <summary>
        /// Initializes the block's rigidbody upon creation.
        /// </summary>
        public void Create() {
            _body = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Randomly assigns a color to the block.
        /// </summary>
        public void Init() {
            GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value, 1);
        }

        /// <summary>
        /// Checks if the block should be despawned from falling out of bounds.
        /// </summary>
        private void Update() {
            if (transform.position.y < PhysicsUtil.DEATHHEIGHT) {
                Despawn();
            }
        }

        /// <summary>
        /// Despawns the block.
        /// </summary>
        public void Despawn() {
            BlockManager.instance.DespawnBlock(this);
        }
    }
}