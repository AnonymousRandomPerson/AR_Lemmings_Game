using UnityEngine;
using Lemmings.Entities.Blocks;
using Lemmings.Managers;
using Lemmings.UI;
using Lemmings.Util;

namespace Lemmings.Entities.Player {

    /// <summary>
    /// Handles player placement of blocks.
    /// </summary>
    class PlayerPlacer : ResettableObject {
        
        /// <summary> The singleton player instance. </summary>
        private static PlayerPlacer playerPlacer;
        /// <summary> The singleton player instance. </summary>
        public static PlayerPlacer instance {
            get { return playerPlacer; }
        }

        /// <summary> The block currently selected by the player. </summary>
        private BlockType _selectedBlock;
        /// <summary> The block currently selected by the player. </summary>
        public BlockType selectedBlock {
            get { return _selectedBlock; }
        }

        /// <summary> The number of types of blocks. </summary>
        private int numBlockTypes;
        /// <summary> The number key of the last available block. </summary>
        private KeyCode keyLimit;

        /// <summary>
        /// Initializes the singleton player placer instance.
        /// </summary>
        private void Awake() {
            playerPlacer = this;
        }

        /// <summary>
        /// Determines the number of types of blocks.
        /// </summary>
        private void Start() {
            numBlockTypes = BlockManager.instance.numTypes;
            if (numBlockTypes >= 9) {
                keyLimit = KeyCode.Alpha9;
            } else {
                keyLimit = KeyCode.Alpha1 + numBlockTypes - 1;
            }
        }

        /// <summary>
        /// Takes user input to manipulate blocks.
        /// </summary>
        private void Update() {
            if (!PauseHandler.instance.paused) {
                SwitchBlock();
                if (InputUtil.GetRightMouseDown()) {
                    PlaceBlock();
                } else if (InputUtil.GetLeftMouseDown()) {
                    RemoveBlock();
                }
            }
        }

        /// <summary>
        /// Places a block where the player is looking at.
        /// </summary>
        private void PlaceBlock() {
            RaycastHit point;
            if (Physics.Raycast(transform.position, transform.forward, out point, 10) && point.collider.tag != "Lemming") {
                BlockManager.instance.SpawnBlock(point.point, transform.eulerAngles, point.normal, selectedBlock);
            }
        }

        /// <summary>
        /// Removes the block that the player is looking at.
        /// </summary>
        private void RemoveBlock() {
            RaycastHit point;
            if (Physics.Raycast(transform.position, transform.forward, out point, 10)) {
                Block block = point.collider.GetComponent<Block>();
                if (block != null) {
                    block.Despawn();
                }
            }
        }

        /// <summary>
        /// Switches the selected block if the user scrolls.
        /// </summary>
        private void SwitchBlock() {
            float scroll = InputUtil.GetScrollWheel();
            if (scroll != 0) {
                if (scroll < 0) {
                    _selectedBlock++;
                    if ((int)_selectedBlock >= numBlockTypes) {
                        _selectedBlock = 0;
                    }
                } else {
                    _selectedBlock--;
                    if (_selectedBlock < 0) {
                        _selectedBlock = (BlockType)(numBlockTypes - 1);
                    }
                }
            } else {
                for (KeyCode key = KeyCode.Alpha1; key <= keyLimit; key++) {
                    if (InputUtil.GetKeyDown(key)) {
                        _selectedBlock = (BlockType)(key - KeyCode.Alpha1);
                    }
                }
            }
        }

        /// <summary>
        /// Resets the object.
        /// </summary>
        public override void Reset() {
            base.Reset();
            _selectedBlock = 0;
        }
    }
}