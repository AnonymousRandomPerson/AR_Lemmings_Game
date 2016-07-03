using UnityEngine;
using System.Collections.Generic;
using Lemmings.Entities.Blocks;
using Lemmings.Enums;
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

        /// <summary> The game object that holds the blocks. </summary>
        private GameObject blockContainer;

        /// <summary> The block prefab to initialize blocks with. </summary>
        [SerializeField]
        [Tooltip("The block prefabs to initialize object with.")]
        private Block[] blockPrefabs;
        /// <summary> The number of blocks in the factory. </summary>
        [SerializeField]
        [Tooltip("The number of blocks in the factory of each type.")]
        private int[] numBlocks;

        /// <summary> The unspawned blocks in the factory. </summary>
        private Queue<Block>[] inactiveBlocks;
        /// <summary> The blocks in the factory. </summary>
        private List<Block>[] blocks;

        /// <summary> The y offsets to spawn blocks at. </summary>
        private float[] blockOffsets;

        /// <summary> The number of block types. </summary>
        private int _numTypes;
        /// <summary> The number of block types. </summary>
        public int numTypes {
            get { return blockPrefabs.Length; }
        }

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
            blockContainer = ObjectUtil.CreateNewObject("Blocks");
            _numTypes = blockPrefabs.Length;
            inactiveBlocks = new Queue<Block>[_numTypes];
            blocks = new List<Block>[_numTypes];
            blockOffsets = new float[_numTypes];
            for (int i = 0; i < numTypes; i++) {
                inactiveBlocks[i] = new Queue<Block>();
                blocks[i] = new List<Block>(numBlocks[i]);
                for (int j = 0; j < numBlocks[i]; j++) {
                    Block block = ObjectUtil.Instantiate(blockPrefabs[i]);
                    block.gameObject.SetActive(false);
                    block.transform.parent = blockContainer.transform;
                    block.type = (BlockType)i;
                    inactiveBlocks[i].Enqueue(block);
                    blocks[i].Add(block);
                }
                blockOffsets[i] = blockPrefabs[i].transform.localScale.y / 2 * 1.1f;
            }
        }

        /// <summary>
        /// Spawns a block.
        /// </summary>
        /// <returns>The spawned block, or null if no blocks are available to spawn.</returns>
        /// <param name="position">The initial position of the block.</param>
        /// <param name="rotation">The initial rotation of the block.</param>
        /// <param name="rotation">The normal of the surface to spawn the block on.</param>
        /// <param name="type">The type of block to spawn.</param>
        public Block SpawnBlock(Vector3 position, Vector3 rotation, Vector3 normal, BlockType type) {
            Block block = null;
            int typeInt = (int)type;
            if (HasBlock(type)) {
                block = inactiveBlocks[typeInt].Dequeue();
                block.transform.position = position + normal * blockOffsets[typeInt];
                rotation.x = blockPrefabs[typeInt].transform.eulerAngles.x;
                rotation.z = blockPrefabs[typeInt].transform.eulerAngles.z;
                block.transform.rotation = Quaternion.Euler(rotation);
                block.Init();
                block.gameObject.SetActive(true);
            }
            return block;
        }

        /// <summary>
        /// Checks if there are idle blocks of a certain type.
        /// </summary>
        /// <returns>Whether there are idle blocks of the specified type.</returns>
        /// <param name="type">The type of block to check for.</param>
        public bool HasBlock(BlockType type) {
            return inactiveBlocks[(int)type].Count > 0;
        }

        /// <summary>
        /// Despawns a block.
        /// </summary>
        /// <param name="block">The block to despawn.</param>
        public void DespawnBlock(Block block) {
            block.gameObject.SetActive(false);
            inactiveBlocks[(int)block.type].Enqueue(block);
        }

        /// <summary>
        /// Resets all blocks in the scene.
        /// </summary>
        public void Reset() {
            foreach (List<Block> blockList in blocks) {
                foreach (Block block in blockList) {
                    if (block.gameObject.activeSelf) {
                        DespawnBlock(block);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of available blocks of a certain type.
        /// </summary>
        /// <returns>The number of available blocks. of the specified type.</returns>
        /// <param name="block">The block type to get the number of available blocks for.</param>
        public int GetNumAvailable(BlockType block) {
            return inactiveBlocks[(int)block].Count;
        }
    }
}