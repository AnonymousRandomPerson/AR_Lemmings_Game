using UnityEngine;

namespace Lemmings.UI {

    /// <summary>
    /// Proxy for supporting different types of sliders.
    /// </summary>
    abstract class SliderBase : MonoBehaviour {

        /// <summary> The value of the underlying slider. </summary>
        internal abstract float sliderValue {
            set;
        }
    }
}