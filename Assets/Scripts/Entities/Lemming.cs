using UnityEngine;
using Lemmings.Entities.Blocks;
using Lemmings.Level;
using Lemmings.Managers;
using Lemmings.Util;

namespace Lemmings.Entities {
    /// <summary>
    /// Characters that must be guided to an exit.
    /// </summary>
    public class Lemming : ResettableObject {

        /// <summary> The game manager in the scene. </summary>
        private GameManager gameManager;

        /// <summary> The movement speed of the lemming. </summary>
        [SerializeField]
        [Tooltip("The movement speed of the lemming.")]
        private float moveSpeed;

        /// <summary> Half of the lemming's width. </summary>
        private float forwardOffset;
        /// <summary> Half of the lemming's length. </summary>
        private float sideOffset;
        /// <summary> Half of the lemming's height. </summary>
        private float heightOffset;
        /// <summary> The distance from a block that will be considered blocking. </summary>
        private float blockDistance;
        /// <summary> Layers that the lemming will collide with. </summary>
        [SerializeField]
        [Tooltip("Layers that the lemming will collide with.")]
        private LayerMask layerMask;
        /// <summary> The collider on the lemming. </summary>
        private Collider mainCollider;
        /// <summary> The rigidbody on the lemming. </summary>
        private Rigidbody _body;
        /// <summary> The rigidbody on the lemming. </summary>
        public Rigidbody body {
            get { return _body; }
        }

        /// <summary> Whether the lemming is dead. </summary>
        private bool dead;
        /// <summary> Whether the lemming has reached the goal. </summary>
        private bool won;
        /// <summary> Whether the lemming is visible. </summary>
        private bool visible = true;

        /// <summary> Renderers on the lemming. </summary>
        private Renderer[] renderers;
        /// <summary> The animator for the lemming's model. </summary>
        private Animator animator;

        /// <summary> The falling speed that will cause the lemming to die. </summary>
        [SerializeField]
        [Tooltip("The falling speed that will cause the lemming to die.")]
        private float fallDeathSpeed;

        /// <summary>
        /// Logs the initial conditions of the object.
        /// </summary>
        protected override void Start() {
            base.Start();
            animator = GetComponentInChildren<Animator>();
            animator.SetBool("moving", true);

            mainCollider = GetComponent<Collider>();
            _body = GetComponent<Rigidbody>();
            forwardOffset = mainCollider.bounds.extents.x;
            sideOffset = mainCollider.bounds.extents.z;
            heightOffset = mainCollider.bounds.extents.y;
            blockDistance = forwardOffset * 1.5f;

            renderers = transform.FindChild("Model").GetComponentsInChildren<MeshRenderer>();

            gameManager = GameManager.instance;
            gameManager.activeLemmings++;
            gameManager.numLemmings++;
            if (gameManager.pictureMode) {
                body.useGravity = false;
            }
        }

        /// <summary>
        /// Updates the lemming every tick.
        /// </summary>
        private void Update() {
            if (dead || won) {
                if (visible) {
                    Disappear();
                }
            }
        }

        /// <summary>
        /// Updates the lemming every physics tick.
        /// </summary>
        private void FixedUpdate() {
            if (!dead && !won && !gameManager.pictureMode) {
                Move();
            }
        }

        /// <summary>
        /// Moves the lemming.
        /// </summary>
        private void Move() {
            // Check if an obstacle is blocking the lemming.
            RaycastHit blocking;
            Vector3 castOrigin = transform.position - Vector3.up * heightOffset / 4;
            if (Physics.Raycast(castOrigin, transform.forward, out blocking, blockDistance, layerMask)) {
            } else if (Physics.Raycast(castOrigin + sideOffset * transform.right, transform.forward, out blocking, blockDistance, layerMask)) {
            } else if (Physics.Raycast(castOrigin - sideOffset * transform.right, transform.forward, out blocking, blockDistance, layerMask)) {
            }
            Block block = null;
            if (blocking.collider != null) {
                block = blocking.collider.GetComponent<Block>();
            }
            if (block != null) {
                block.AffectLemming(this, blocking);
            }

            transform.Translate(Vector3.forward * moveSpeed, Space.Self);

            if (transform.position.y < PhysicsUtil.DEATHHEIGHT) {
                Die();
            }
        }

        /// <summary>
        /// Handles the disappearing animation for the lemming.
        /// </summary>
        private void Disappear() {
            _body.velocity = Vector3.zero;
            Color playerColor = new Color();
            foreach (Renderer playerRenderer in renderers) {
                playerColor = playerRenderer.material.color;
                playerColor.a -= Time.deltaTime;
                playerRenderer.material.color = playerColor;
            }
            if (playerColor.a <= Mathf.Epsilon) {
                foreach (Renderer playerRenderer in renderers) {
                    playerRenderer.enabled = false;
                }
                visible = false;
                gameManager.activeLemmings--;
            }
        }

        /// <summary>
        /// Kills the lemming.
        /// </summary>
        public void Die() {
            dead = true;
            mainCollider.enabled = false;
            animator.speed = 0;
            _body.useGravity = false;
        }

        /// <summary>
        /// Marks that the lemming has reached the goal.
        /// </summary>
        private void Win() {
            won = true;
            mainCollider.enabled = false;
            _body.useGravity = false;
            animator.SetBool("moving", false);
            gameManager.goalLemmings++;
        }

        /// <summary>
        /// Checks for collisions with the lemming.
        /// </summary>
        /// <param name="collision">The collision that occurred.</param>
        private void OnCollisionEnter(Collision collision) {
            if (collision.collider.tag == "Finish") {
                collision.collider.GetComponent<Goal>().Win();
                Win();
            } else if (Mathf.Abs(collision.relativeVelocity.y) > fallDeathSpeed) {
                Die();
            }
        }

        /// <summary>
        /// Resets the object.
        /// </summary>
        public override void Reset() {
            base.Reset();

            dead = false;
            won = false;

            visible = true;
            Color playerColor = new Color();
            foreach (Renderer playerRenderer in renderers) {
                playerColor = playerRenderer.material.color;
                playerColor.a = 1;
                playerRenderer.material.color = playerColor;
                playerRenderer.enabled = true;
            }
            animator.speed = 1;
            animator.SetBool("moving", true);
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0);

            mainCollider.enabled = true;
            _body.useGravity = true;
            _body.velocity = Vector3.zero;

            gameManager.activeLemmings++;
        }
    }
}