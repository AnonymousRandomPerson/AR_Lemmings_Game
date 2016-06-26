using UnityEngine;
using Lemmings.Entities;
using Lemmings.Util;

namespace Lemmings.Graphics {
    /// <summary>
    /// The portal that lemmings spawn from.
    /// </summary>
    public class Portal : ResettableObject {

        /// <summary> The maximum size of the portal. </summary>
        private float maxPortalSize;
        /// <summary> The rate at which the portal expands. </summary>
        [SerializeField]
        [Tooltip("The rate at which the portal expands.")]
        private float expandRate;
        /// <summary> The speed that the portal rotates at. </summary>
        [SerializeField]
        [Tooltip("The speed that the portal rotates at.")]
        private float rotateSpeed;

        /// <summary> Animation states of the portal. </summary>
        private enum State { Expand, Active, Shrink }
        /// <summary> The current animation state of the portal.v </summary>
        private State state;

        /// <summary>
        /// Initializes the portal size.
        /// </summary>
        protected override void Start() {
            base.Start();
            maxPortalSize = transform.localScale.x;
            Reset();
        }
        
        /// <summary>
        /// Handles animations for the portal.
        /// </summary>
        private void Update() {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
            if (state == State.Expand) {
                transform.localScale = VectorUtil.ScaleXZ(transform.localScale, expandRate * Time.deltaTime);
                if (transform.localScale.x >= maxPortalSize) {
                    state = State.Active;
                    transform.localScale = VectorUtil.SetXZ(transform.localScale, maxPortalSize);
                }
            } else if (state == State.Shrink) {
                transform.localScale = VectorUtil.ScaleXZ(transform.localScale, -expandRate * Time.deltaTime);
                if (transform.localScale.x <= 0) {
                    gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Checks if the portal is ready to spawn lemmings.
        /// </summary>
        public bool IsReady() {
            return state != State.Expand;
        }

        /// <summary>
        /// Causes the portal to start shrinking.
        /// </summary>
        public void SetShrink() {
            state = State.Shrink;
        }

        /// <summary>
        /// Resets the object.
        /// </summary>
        public override void Reset() {
            base.Reset();
            transform.localScale = VectorUtil.SetXZ(transform.localScale, 0);
            state = State.Expand;
            gameObject.SetActive(true);
        }
    }
}