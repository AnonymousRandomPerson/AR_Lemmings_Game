using UnityEngine;
using Lemmings.Entities;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.Entities.Player {
    /// <summary>
    /// Handles player movement.
    /// </summary>
    class PlayerMover : ResettableObject {

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

        /// <summary> The player controller. </summary>
        private CharacterController controller;

        /// <summary>
        /// Initializes the player controller.
        /// </summary>
        protected override void Start() {
            base.Start();
            controller = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Updates the player every physics tick.
        /// </summary>
        private void FixedUpdate() {
            Move();

            if (transform.position.y < PhysicsUtil.DEATH_HEIGHT) {
                GameManager.instance.ResetLevel();
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
            targetDirection = transform.rotation * targetDirection;
            targetDirection.y = 0;
            targetDirection.Normalize();
            targetDirection *= maxSpeed;
            moveDirection = Vector3.MoveTowards(moveDirection, targetDirection, acceleration);
            controller.SimpleMove(moveDirection);
        }

        /// <summary>
        /// Resets the object.
        /// </summary>
        public override void Reset() {
            GetComponent<PlayerPlacer>().Reset();
            base.Reset();
            moveDirection = Vector3.zero;
        }
    }
}