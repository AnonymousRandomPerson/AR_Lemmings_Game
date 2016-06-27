using UnityEngine;

namespace Lemmings.Util.Timers {
    
    /// <summary> A function that can only execute once within a certain delay. </summary>
    public delegate void LimitCallback();

    /// <summary>
    /// Causes a function to only be executable once within a certain delay.
    /// </summary>
    public class LimitTimerCallback {

        /// <summary> The function to limit calls to. </summary>
        private LimitCallback callback;

        /// <summary> Timer that keeps track of when the function can be called. </summary>
        private LimitTimer limitTimer;

        /// <summary>
        /// Initializes a limit function.
        /// </summary>
        /// <param name="callback">The function to limit calls to.</param>
        /// <param name="timeDelay">The time delay before the function can be called again.</param>
        public LimitTimerCallback(LimitCallback callback, float timeDelay) {
            limitTimer = new LimitTimer(timeDelay);
            this.callback = callback;
        }

        /// <summary>
        /// Calls the function if the delay is over.
        /// </summary>
        public void Run() {
            if (limitTimer.CanRun()) {
                callback();
            }
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        public void Reset() {
            limitTimer.Reset();
        }
    }
}

