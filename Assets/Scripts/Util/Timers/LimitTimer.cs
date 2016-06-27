using UnityEngine;

namespace Lemmings.Util.Timers {
    /// <summary>
    /// Keeps track of the last time that an action occurred.
    /// </summary>
    public class LimitTimer {

        /// <summary> The time that the function was last called at. </summary>
        protected float lastTime;
        /// <summary> The time delay before the function can be called again. </summary>
        protected float timeDelay;

        /// <summary>
        /// Initializes a timer.
        /// </summary>
        /// <param name="timeDelay">The time delay before the action can be called again.</param>
        public LimitTimer(float timeDelay) {
            lastTime = -timeDelay;
            this.timeDelay = timeDelay;
        }

        /// <summary>
        /// Checks if the action can occur.
        /// </summary>
        /// <returns>Whether the action can occur..</returns>
        public bool CanRun() {
            float currentTime = Time.time;
            if (currentTime - lastTime >= timeDelay) {
                lastTime = currentTime;
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        public void Reset() {
            lastTime = -timeDelay;
        }
    }
}

