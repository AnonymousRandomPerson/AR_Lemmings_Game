using UnityEngine;
using Lemmings.Entities.Blocks;
using Lemmings.Level;
using Lemmings.Managers;
using Lemmings.Util;
using Lemmings.Util.Timers;

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
        /// <summary> Whether the lemming has finished spawning. </summary>
        private bool spawned;

        /// <summary> Renderers on the lemming. </summary>
        private Renderer[] renderers;
        /// <summary> The animator for the lemming's model. </summary>
        private Animator animator;

        /// <summary> The falling speed that will cause the lemming to die. </summary>
        [SerializeField]
        [Tooltip("The falling speed that will cause the lemming to die.")]
        private float fallDeathSpeed;

        /// <summary> Timer for limiting jump pad effects. </summary>
        private LimitTimer jumpTimer;

        /// <summary> The spawner that spawned the lemming. </summary>
        private LemmingSpawner spawner;

        /// <summary> The unique index of the lemming. </summary>
        private int _index;
        /// <summary> The unique index of the lemming. </summary>
        public int index {
            get { return _index; }
        }

        /// <summary> The height of a lemming. </summary>
        private static float _height;
        /// <summary> The height of a lemming. </summary>
        public static float height {
            get { return _height; }
        }

        /// <summary>
        /// Logs the initial conditions of the object.
        /// </summary>
        protected override void Start() {
            base.Start();

            mainCollider = GetComponent<Collider>();
            _body = GetComponent<Rigidbody>();
            forwardOffset = mainCollider.bounds.extents.x;
            sideOffset = mainCollider.bounds.extents.z;
            heightOffset = mainCollider.bounds.extents.y;
            _height = heightOffset * 2;
            blockDistance = forwardOffset * 1.5f;

            animator = GetComponentInChildren<Animator>();
            renderers = transform.FindChild("Model").GetComponentsInChildren<MeshRenderer>();

            jumpTimer = new LimitTimer(0.25f);
            ResetFields();
        }

        /// <summary>
        /// Initializes the lemming when spawning it.
        /// </summary>
        internal void Spawn(LemmingSpawner spawner, int index) {
            visible = true;
            this.spawner = spawner;
            _index = index;
            gameManager = GameManager.instance;
            gameManager.AddLemming(this);
        }

        /// <summary>
        /// Updates the lemming every tick.
        /// </summary>
        private void Update() {
            if (dead || won) {
                if (visible) {
                    Disappear();
                }
            } else if (!spawned) {
                Appear();
            }
        }

        /// <summary>
        /// Updates the lemming every physics tick.
        /// </summary>
        private void FixedUpdate() {
            if (!dead && !won && spawned && !gameManager.pictureMode) {
                Move();
                CheckSurface();
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
                if (block != null) {
                    block.AffectLemming(this, blocking);
                } else {
                    transform.RotateAround(transform.position, transform.up, 180);
                }
            }

            if (block == null) {
                transform.Translate(Vector3.forward * moveSpeed, Space.Self);
            }

            if (transform.position.y < PhysicsUtil.DEATH_HEIGHT) {
                Die();
            }
        }

        /// <summary>
        /// Checks whether the surface underneath has been visited.
        /// </summary>
        private void CheckSurface() {
            RaycastHit surfaceHit;
            LayerMask surfaceMask = 1 << LayerMask.NameToLayer("Platform");
            if (Physics.Raycast(transform.position, Vector3.down, out surfaceHit, heightOffset * 2, surfaceMask)) {
                Surface surface = surfaceHit.collider.GetComponentInParent<Surface>();
                if (surface != null) {
                    surface.VisitSurface();
                }
            }
        }

        /// <summary>
        /// Appear this instance.
        /// </summary>
        private void Appear() {
            _body.velocity = Vector3.zero;
            if (AddAlpha(Time.deltaTime) >= 1) {
                mainCollider.enabled = true;
                _body.useGravity = true;
                spawned = true;

                animator.speed = 1;
                animator.SetBool("moving", true);
                animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0);

                spawner.FinishSpawn();
            }
        }

        /// <summary>
        /// Handles the disappearing animation for the lemming.
        /// </summary>
        private void Disappear() {
            _body.velocity = Vector3.zero;
            if (AddAlpha(-Time.deltaTime) <= Mathf.Epsilon) {
                foreach (Renderer playerRenderer in renderers) {
                    playerRenderer.enabled = false;
                }
                visible = false;
                gameManager.RemoveLemming(this);
            }
        }

        /// <summary>
        /// Adds an alpha value to the player renderers.
        /// </summary>
        /// <returns>The new alpha value of the player renderers.</returns>
        /// <param name="amount">The alpha value to add.</param>
        private float AddAlpha(float amount) {
            Color playerColor = new Color();
            foreach (Renderer playerRenderer in renderers) {
                playerColor = playerRenderer.material.color;
                playerColor.a += amount;
                playerRenderer.material.color = playerColor;
            }
            return playerColor.a;
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
        /// Checks if the lemming can be affected by a jump pad.
        /// </summary>
        /// <returns>Whether the lemming can be affected by a jump pad.</returns>
        public bool CanJump() {
            return jumpTimer.CanRun();
        }

        /// <summary>
        /// Resets the object.
        /// </summary>
        public override void Reset() {
            base.Reset();

            ResetFields();
            jumpTimer.Reset();

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Resets the fields present on the lemming.
        /// </summary>
        private void ResetFields() {
            dead = false;
            won = false;
            spawned = false;

            Color playerColor = new Color();
            foreach (Renderer playerRenderer in renderers) {
                playerColor = playerRenderer.material.color;
                playerColor.a = 0;
                playerRenderer.material.color = playerColor;
                playerRenderer.enabled = true;
            }

            mainCollider.enabled = false;
            _body.useGravity = false;
            _body.velocity = Vector3.zero;
        }
    }
}