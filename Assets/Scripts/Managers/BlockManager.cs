using UnityEngine;
using System.Collections.Generic;
using Lemmings.Entities;
using Lemmings.Util;

namespace Lemmings.Managers {
    /// <summary>
    /// Factory for blocks.
    /// </summary>
    class BlockManager : MonoBehaviour {

        /// <summary> The singleton instance of the block manager. </summary>
        private static BlockManager _instance;
        /// <summary> The singleton instance of the block manager. </summary>
        public static BlockManager instance {
            get {
                return _instance;
            }
        }

        /// <summary> The block prefab to initialize blocks with. </summary>
        [SerializeField]
        [Tooltip("The block prefab to initialize blocks with.")]
        private Block blockPrefab;
        /// <summary> The number of blocks in the factory. </summary>
        [SerializeField]
        [Tooltip("The number of blocks in the factory.")]
        private int numBlocks;
        /// <summary> The number of blocks in the factory. </summary>
        public int NumBlocks {
            get { return numBlocks; }
        }
        /// <summary> The number of blocks that have been placed on the field. </summary>
        public int placedBlocks {
            get { return numBlocks - inactiveBlocks.Count; }
        }

        /// <summary> The unspawned blocks in the factory. </summary>
        private Queue<Block> inactiveBlocks;
        /// <summary> The blocks in the factory. </summary>
        private List<Block> blocks;

        /// <summary> The y offset to spawn a block at. </summary>
        private float blockOffset;
        /// <summary>
        /// Sets the singleton block manager instance.
        /// </summary>
        private void Awake() {
            _instance = this;
        }

        /// <summary>
        /// Creates the blocks in the factory.
        /// </summary>
        private void Start() {
            inactiveBlocks = new Queue<Block>();
            blocks = new List<Block>(numBlocks);
            for (int i = 0; i < numBlocks; i++) {
                Block block = GameObject.Instantiate(blockPrefab);
                block.gameObject.SetActive(false);
                block.transform.parent = transform;
                block.name = StringUtil.RemoveClone(block.name);
                block.Create();
                inactiveBlocks.Enqueue(block);
                blocks.Add(block);
            }
            blockOffset = blockPrefab.transform.localScale.y / 2 * 1.1f;
        }

        /// <summary>
        /// Spawns a block.
        /// </summary>
        /// <returns>The spawned block, or null if no blocks are available to spawn.</returns>
        /// <param name="position">The initial position of the block.</param>
        /// <param name="rotation">The initial rotation of the block.</param>
        /// <param name="rotation">The normal of the surface to spawn the block on..</param>
        public Block SpawnBlock(Vector3 position, Vector3 rotation, Vector3 normal) {
            Block block = null;
            if (inactiveBlocks.Count > 0) {
                block = inactiveBlocks.Dequeue();
                block.transform.position = position + normal * blockOffset;
                rotation.x = 0;
                rotation.z = 0;
                block.transform.rotation = Quaternion.Euler(rotation);
                block.Init();
                block.body.velocity = Vector3.zero;
                block.gameObject.SetActive(true);
            }
            return block;
        }

        /// <summary>
        /// Despawns a block.
        /// </summary>
        /// <param name="block">The block to despawn.</param>
        public void DespawnBlock(Block block) {
            block.gameObject.SetActive(false);
            inactiveBlocks.Enqueue(block);
        }

        /// <summary>
        /// Resets all blocks in the scene.
        /// </summary>
        public void Reset() {
            foreach (Block block in blocks) {
                if (block.gameObject.activeSelf) {
                    DespawnBlock(block);
                }
            }
        }
    }
}