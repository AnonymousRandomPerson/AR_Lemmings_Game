using UnityEngine;
using Lemmings.Entities;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.Entities.Player {
    /// <summary>
    /// Handles player movement.
    /// </summary>
    class PlayerMover : ResettableObject {

        /// <summary> The singleton player instance. </summary>
        private static PlayerMover _instance;
        /// <summary> The singleton player instance. </summary>
        public static PlayerMover instance {
            get { return _instance; }
        }

        public static string POSITION_KEY = "user_location";

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

        /// <summary> Whether the player can fly and pass through obstacles. </summary>
        [Tooltip("Whether the player can fly and pass through obstacles.")]
        public bool noClip;
        /// <summary> Whether the player started out in no-clip mode. </summary>
        private bool startNoClip;

        /// <summary> Whether to use the Oculus camera controls. </summary>
        private bool vrCamera;

        /// <summary> The first-person camera attached to the player. </summary>
        [SerializeField]
        [Tooltip("The first-person camera attached to the player.")]
        private Camera playerCamera;

        /// <summary> The height of the player. </summary>
        public float height {
            get {
                return controller.height;
            }
        }

        /// <summary> The player controller. </summary>
        private CharacterController controller;

        /// <summary>
        /// Initializes the singleton player instance.
        /// </summary>
        private void Awake() {
            _instance = this;
        }

        /// <summary>
        /// Initializes the player controller.
        /// </summary>
        protected override void Start() {
            base.Start();
            controller = GetComponent<CharacterController>();
            startNoClip = noClip;
            if (playerCamera == null) {
                playerCamera = GetComponentInChildren<Camera>();
            }
            vrCamera = GameManager.instance.GetComponent<VRSwitcher>().vrEnabled;
        }

        /// <summary>
        /// Updates the player every physics tick.
        /// </summary>
        private void FixedUpdate() {
            gameObject.layer = LayerMask.NameToLayer(noClip ? "Player NoClip" : "Player");
            if (!GameManager.instance.isLoading) {
                Move();

                if (transform.position.y < PhysicsUtil.DEATH_HEIGHT) {
                    GameManager.instance.ResetLevel();
                }
            }
        }

        /// <summary>
        /// Moves the player around when keys are pressed.
        /// </summary>
        private void Move() {
            if (!vrCamera) {
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
            }

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

            targetDirection = RotateFacing(targetDirection);
            targetDirection.y = 0;
            targetDirection.Normalize();

            if (noClip) {
                if (InputUtil.GetKey(KeyCode.LeftShift, KeyCode.RightShift, KeyCode.PageDown)) {
                    targetDirection += Vector3.down;
                }
                if (InputUtil.GetKey(KeyCode.Space, KeyCode.PageUp)) {
                    targetDirection += Vector3.up;
                }
                targetDirection.Normalize();
            }

            targetDirection *= maxSpeed;
            moveDirection = Vector3.MoveTowards(moveDirection, targetDirection, acceleration);

            if (noClip) {
                controller.Move(moveDirection * Time.fixedDeltaTime);
            } else {
                controller.SimpleMove(moveDirection);
            }
        }

        /// <summary>
        /// Rotates a vector by the direction the player is facing.
        /// </summary>
        /// <returns>The rotated vector.</returns>
        /// <param name="vector">The vector to rotate.</param>
        internal Vector3 RotateFacing(Vector3 vector) {
            return playerCamera.transform.rotation * vector;
        }

        /// <summary>
        /// Resets the object.
        /// </summary>
        public override void Reset() {
            GetComponent<PlayerPlacer>().Reset();
            base.Reset();
            moveDirection = Vector3.zero;
            noClip = startNoClip;
        }
    }
}