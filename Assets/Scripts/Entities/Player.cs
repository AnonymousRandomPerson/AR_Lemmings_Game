using UnityEngine;
using Lemmings.Entities.Blocks;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.Entities {
    /// <summary>
    /// The player controller.
    /// </summary>
    class Player : ResettableObject {

        /// <summary> The singleton player instance. </summary>
        private static Player player;
        /// <summary> The singleton player instance. </summary>
        public static Player instance {
            get { return player; }
        }

        /// <summary> The direction that the player is moving in. </summary>
        private Vector3 moveDirection;
        /// <summary> The maximum movement speed of the player. </summary>
        [SerializeField]
        [Tooltip("The maximum movement speed of the player.")]
        private float maxSpeed;
        /// <summary> The maximum acceleration of the player. </summary>
        [SerializeField]
        [Tooltip("The maximum acceleration of the player.")]
        private float acceleration;

        /// <summary> The maximum pitch of the camera. </summary>
        [SerializeField]
        [Tooltip("The maximum pitch of the camera.")]
        private float maxPitch;
        /// <summary> The turn speed of the camera. </summary>
        [SerializeField]
        [Tooltip("The turn speed of the camera.")]
        private float turnSpeed;

        /// <summary> The block currently selected by the player. </summary>
        private BlockType _selectedBlock;
        /// <summary> The block currently selected by the player. </summary>
        public BlockType selectedBlock {
            get { return _selectedBlock; }
        }

        /// <summary>
        /// Initializes the singleton player instance.
        /// </summary>
        private void Awake() {
            player = this;
        }

        /// <summary>
        /// Places blocks when the user clicks the left mouse button.
        /// </summary>
        private void Update() {
            SwitchBlock();
            if (InputUtil.GetLeftMouseDown()) {
                PlaceBlock();
            } else if (InputUtil.GetRightMouseDown()) {
                RemoveBlock();
            }
        }

        /// <summary>
        /// Updates the player every physics tick.
        /// </summary>
        private void FixedUpdate() {
            Move();
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
                    if (selectedBlock >= BlockType.NumTypes) {
                        _selectedBlock = 0;
                    }
                } else {
                    _selectedBlock--;
                    if (_selectedBlock < 0) {
                        _selectedBlock = BlockType.NumTypes - 1;
                    }
                }
            }
        }

        /// <summary>
        /// Moves the player around when keys are pressed.
        /// </summary>
        private void Move() {
            Vector2 mouseMovement = InputUtil.GetMouseMovement() * turnSpeed;
            transform.Rotate(-mouseMovement.y, mouseMovement.x, 0);
            Vector3 rotation = transform.eulerAngles;
            rotation.z = 0;
            if (rotation.x > 180) {
                rotation.x = Mathf.Max(rotation.x, 360 - maxPitch);
            } else {
                rotation.x = Mathf.Min(rotation.x, maxPitch);
            }
            transform.eulerAngles = rotation;

            Vector3 targetDirection = Vector3.zero;
            if (InputUtil.GetKey(KeyCode.D, KeyCode.RightArrow)) {
                targetDirection += Vector3.right;
            }
            if (InputUtil.GetKey(KeyCode.A, KeyCode.LeftArrow)) {
                targetDirection += Vector3.left;
            }
            if (InputUtil.GetKey(KeyCode.W, KeyCode.UpArrow)) {
                targetDirection += Vector3.forward;
            }
            if (InputUtil.GetKey(KeyCode.S, KeyCode.DownArrow)) {
                targetDirection += Vector3.back;
            }
            if (InputUtil.GetKey(KeyCode.LeftShift, KeyCode.RightShift, KeyCode.PageDown)) {
                targetDirection += Vector3.down;
            }
            if (InputUtil.GetKey(KeyCode.Space, KeyCode.PageUp)) {
                targetDirection += Vector3.up;
            }
            targetDirection *= maxSpeed;
            moveDirection = Vector3.MoveTowards(moveDirection, targetDirection, acceleration);
            transform.Translate(moveDirection.x, 0, moveDirection.z, Space.Self);
            transform.Translate(Vector3.up * moveDirection.y, Space.World);
        }

        /// <summary>
        /// Resets the object.
        /// </summary>
        public override void Reset() {
            base.Reset();
            moveDirection = Vector3.zero;
        }
    }
}