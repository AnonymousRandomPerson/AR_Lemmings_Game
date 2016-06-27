using UnityEngine;
using Lemmings.Graphics;
using Lemmings.Managers;
using Lemmings.Util;
using Lemmings.Util.Timers;

namespace Lemmings.Entities {
    /// <summary>
    /// Spawns lemmings at the start of the level.
    /// </summary>
    class LemmingSpawner : ResettableObject {

        /// <summary> Lemming resource to be instantiated from. </summary>
        [SerializeField]
        [Tooltip("Lemming resource to be instantiated from.")]
        private Lemming lemmingPrefab;

        /// <summary> The lemmings in the spawner. </summary>
        private Lemming[] lemmings;
        /// <summary> The total number of lemmings to spawn. </summary>
        private int _totalLemmings;
        /// <summary> The total number of lemmings to spawn. </summary>
        public int totalLemmings {
            set {
                _totalLemmings = value;
                lemmings = new Lemming[value];
                GameManager.instance.numLemmings = GameManager.instance.numLemmings + value;
                for (int i = 0; i < value; i++) {
                    lemmings[i] = ObjectUtil.Instantiate(lemmingPrefab);
                    lemmings[i].gameObject.SetActive(false);
                }
            }
        }
        /// <summary> The number of lemmings that have been spawned. </summary>
        private int spawnedLemmings;
        /// <summary> The number of lemmings that have finished being spawned. </summary>
        private int finishedSpawnedLemmings;

        /// <summary> The amount of time between lemming spawns. </summary>
        [SerializeField]
        [Tooltip("The amount of time between lemming spawns.")]
        private float spawnInterval;
        /// <summary> Timer for spawning lemmings. </summary>
        private LimitTimerCallback spawnTimer;
        /// <summary> The y offset to the spawn point. </summary>
        private float spawnOffset;

        /// <summary> The visible portal of the spawner. </summary>
        private Portal portal;

        /// <summary>
        /// Initializes the spawner.
        /// </summary>
        protected override void Start() {
            base.Start();
            spawnTimer = new LimitTimerCallback(SpawnLemming, spawnInterval);
            portal = transform.GetComponentInChildren<Portal>();
        }
        
        /// <summary>
        /// Periodically spawns lemmings up to the spawn limit.
        /// </summary>
        private void Update() {
            if (!IsFinished() && portal.IsReady()) {
                spawnTimer.Run();
            }
        }

        /// <summary>
        /// Checks if the spawner has finished spawning lemmings.
        /// </summary>
        /// <returns>Whether the spawner has finished spawning lemmings.</returns>
        public bool IsFinished() {
            return spawnedLemmings >= _totalLemmings;
        }

        /// <summary>
        /// Spawns a lemming.
        /// </summary>
        private void SpawnLemming() {
            Lemming currentLemming = lemmings[spawnedLemmings++];
            currentLemming.gameObject.SetActive(true);
            if (spawnOffset == 0) {
                spawnOffset = currentLemming.GetComponent<Collider>().bounds.extents.y;
            }
            currentLemming.transform.position = transform.position + Vector3.up * spawnOffset * 1.5f;
            currentLemming.Spawn(this);
            GameManager.instance.activeLemmings++;
        }

        /// <summary>
        /// Marks a lemming as finished spawning.
        /// </summary>
        public void FinishSpawn() {
            if (++finishedSpawnedLemmings >= _totalLemmings) {
                portal.SetShrink();
            }
        }

        /// <summary>
        /// Resets the object.
        /// </summary>
        public override void Reset() {
            base.Reset();
            spawnedLemmings = 0;
            finishedSpawnedLemmings = 0;
            spawnTimer.Reset();
        }
    }
}