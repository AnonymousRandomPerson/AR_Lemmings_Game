using UnityEngine;
using UnityEngine.VR;
using Lemmings.Entities.Blocks;
using Lemmings.Enums;
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

        /// <summary> The block manager to place blocks with. </summary>
        private BlockManager blockManager;

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

        /// <summary> Layers to ignore when placing blocks. </summary>
        [SerializeField]
        [Tooltip("Layers to ignore when placing blocks.")]
        private LayerMask layerMask;

        /// <summary> The ranch at which blocks can be placed. </summary>
        [SerializeField]
        [Tooltip("The ranch at which blocks can be placed.")]
        private float placeRange;

        /// <summary> Block outlines to indicate where blocks are placed. </summary>
        private GameObject[] ghostBlocks;
        /// <summary> The material for ghost outline blocks. </summary>
        [SerializeField]
        [Tooltip("The material for ghost outline blocks.")]
        private Material ghostMaterial;

        /// <summary> The first-person player camera. </summary>
        private Transform playerCamera;

        /// <summary> The time after placing a block before it can be rotated. </summary>
        [SerializeField]
        [Tooltip("The time after placing a block before it can be rotated.")]
        private float placeCooldown;
        /// <summary> Timer for delaying block rotation. </summary>
        private float placeTimer;

        /// <summary>
        /// Initializes the singleton player placer instance.
        /// </summary>
        private void Awake() {
            playerPlacer = this;
        }

        /// <summary>
        /// Determines the number of types of blocks.
        /// </summary>
        protected override void Start() {
            base.Start();
            blockManager = BlockManager.instance;
            playerCamera = transform.Find("Main Camera");
            numBlockTypes = blockManager.numTypes;
            if (numBlockTypes >= 9) {
                keyLimit = KeyCode.Alpha9;
            } else {
                keyLimit = KeyCode.Alpha1 + numBlockTypes - 1;
            }

            ghostBlocks = new GameObject[numBlockTypes];
            for (int i = 0; i < numBlockTypes; i++) {
                Block block = blockManager.blockObjects[i];
                GameObject ghostBlock = ObjectUtil.Instantiate(block).gameObject;
                Destroy(ghostBlock.GetComponent<Rigidbody>());
                Destroy(ghostBlock.GetComponent<BoxCollider>());
                Destroy(ghostBlock.GetComponent<Block>());
                ghostBlock.GetComponent<Renderer>().material = ghostMaterial;
                ghostBlock.SetActive(false);
                ghostBlocks[i] = ghostBlock;
            }
        }

        /// <summary>
        /// Takes user input to manipulate blocks.
        /// </summary>
        private void Update() {
            placeTimer -= Time.deltaTime;
            if (!PauseHandler.instance.paused && !GameManager.instance.isCountingDown) {
                SwitchBlock();
                if (InputUtil.GetButtonDown(InputCode.PlaceBlock)) {
                    PlaceBlock();
                } else if (InputUtil.GetButtonDownOrRightMouse(InputCode.RemoveBlock)) {
                    RemoveBlock();
                }
            }
        }

        /// <summary>
        /// Gets the point that the player is looking at.
        /// </summary>
        /// <returns>Whether there is an object at the point the player is looking at.</returns>
        /// <param name="hit">The point that the player is looking at.</param>
        private bool GetTarget(out RaycastHit hit) {
            return Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, placeRange, layerMask);
        }

        /// <summary>
        /// Sets whether the ghost block is visible or not.
        /// </summary>
        /// <param name="visibility">Whether the ghost block is visible or not.</param>
        private void SetGhostBlockVisibility(bool visibility) {
            ghostBlocks[(int)selectedBlock].SetActive(visibility);
        }

        /// <summary>
        /// Gets the player's block placing status.
        /// </summary>
        /// <returns>The player's block placing status.</returns>
        public PlaceStatus GetPlaceStatus() {
            SetGhostBlockVisibility(false);
            RaycastHit point;
            bool depleted = blockManager == null || !blockManager.HasBlock(selectedBlock);
            if (GetTarget(out point) && point.collider.tag != "Lemming") {
                if (point.collider.tag == "Block") {
                    return PlaceStatus.Rotate;
                } else if (depleted) {
                    return PlaceStatus.Out;
                } else {
                    SetGhostBlockVisibility(true);
                    blockManager.MoveBlock(ghostBlocks[(int)selectedBlock], point.point, playerCamera.eulerAngles, point.normal, selectedBlock);
                    return PlaceStatus.Able;
                }
            } else {
                return depleted ? PlaceStatus.Out : PlaceStatus.Range;
            }
        }

        /// <summary>
        /// Places a block where the player is looking at.
        /// </summary>
        private void PlaceBlock() {
            RaycastHit point;
            if (GetTarget(out point) && point.collider.tag != "Lemming") {
                if (point.collider.tag == "Block") {
                    if (placeTimer <= 0) {
                        Block block = point.collider.GetComponent<Block>();
                        block.Rotate();
                    }
                } else {
                    blockManager.SpawnBlock(point.point, playerCamera.eulerAngles, point.normal, selectedBlock);
                    placeTimer = placeCooldown;
                }
            }
        }

        /// <summary>
        /// Removes the block that the player is looking at.
        /// </summary>
        private void RemoveBlock() {
            RaycastHit point;
            if (GetTarget(out point)) {
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
            SetGhostBlockVisibility(false);

            float scroll = InputUtil.GetScrollWheel();
            if (scroll != 0) {
                if (scroll < 0) {
                    _selectedBlock++;
                    if ((int)_selectedBlock >= numBlockTypes) {
                        _selectedBlock--;
                    }
                } else {
                    _selectedBlock--;
                    if (_selectedBlock < 0) {
                        _selectedBlock = 0;
                    }
                }
            } else if (InputUtil.GetButtonDown(InputCode.SwitchBlock)) {
                _selectedBlock++;
                if ((int)_selectedBlock >= numBlockTypes) {
                    _selectedBlock = 0;
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
            SetGhostBlockVisibility(false);
            _selectedBlock = 0;
        }
    }
}