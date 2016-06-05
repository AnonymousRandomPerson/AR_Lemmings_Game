using UnityEngine;

namespace Lemmings.Util {
    /// <summary> A function that can only execute once within a certain delay. </summary>
    public delegate void LimitCallback();

    /// <summary>
    /// Causes a function to only be executable once within a certain delay.
    /// </summary>
    public class LimitTimer {

        /// <summary> The function to limit calls to. </summary>
        private LimitCallback callback;

        /// <summary> The time that the function was last called at. </summary>
        private float lastTime;
        /// <summary> The time delay before the function can be called again. </summary>
        private float timeDelay;

        /// <summary>
        /// Initializes a limit function.
        /// </summary>
        /// <param name="callback">The function to limit calls to.</param>
        /// <param name="timeDelay">The time delay before the function can be called again.</param>
        public LimitTimer(LimitCallback callback, float timeDelay) {
            lastTime = Mathf.NegativeInfinity;
            this.timeDelay = timeDelay;
            this.callback = callback;
        }

        /// <summary>
        /// Calls the function if the delay is over.
        /// </summary>
        public void Run() {
            float currentTime = Time.time;
            if (currentTime - lastTime >= timeDelay) {
                callback();
                lastTime = currentTime;
            }
        }
    }
}

