using UnityEngine;
using Lemmings.Entities;

namespace Lemmings.Level {
    /// <summary>
    /// The goal that the lemmings need to reach.
    /// </summary>
    class Goal : ResettableObject {

        /// <summary> The particles emitted from the goal. </summary>
        private ParticleSystem particles;
        /// <summary> The amount of time that particles will be emitted for. </summary>
        [SerializeField]
        [Tooltip("The amount of time that particles will be emitted for.")]
        private float particleTime;
        /// <summary> Keeps track of how long particles have been emitted for. </summary>
        private float particleTimer;

        /// <summary>
        /// Initializes the goal particle system.
        /// </summary>
        protected override void Start() {
            base.Start();
            particles = transform.GetComponentInChildren<ParticleSystem>();
        }

        /// <summary>
        /// Times particle emission.
        /// </summary>
        private void Update() {
            if (particleTimer > 0) {
                particleTimer -= Time.deltaTime;
                if (particleTimer <= 0) {
                    particles.Stop();
                }
            }
        }

        /// <summary>
        /// Causes the goal to emit particles after .
        /// </summary>
        public void Win() {
            particleTimer = particleTime;
            particles.Play();
            GetComponent<AudioSource>().Play();
        }

        /// <summary>
        /// Disables the goal particles.
        /// </summary>
        public override void Reset() {
            particleTimer = 0;
            particles.Stop();
        }
    }
}