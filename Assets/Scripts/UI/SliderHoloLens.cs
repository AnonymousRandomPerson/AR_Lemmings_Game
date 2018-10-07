using UnityEngine;
using HoloToolkit.Examples.InteractiveElements;

namespace Lemmings.UI {

    /// <summary>
    /// Proxy for the HoloLens slider.
    /// </summary>
    class SliderHoloLens : SliderBase {

        /// <summary> The HoloLens slider attached to this object. </summary>
        private SliderGestureControl slider;

        /// <summary> The value of the underlying slider. </summary>
        internal override float sliderValue {
            set { slider.SliderValue = value; }
        }

        /// <summary>
        /// Finds the Unity slider component.
        /// </summary>
        private void Start() {
            slider = GetComponent<SliderGestureControl>();
        }
    }
}