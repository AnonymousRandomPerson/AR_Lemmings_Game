using UnityEngine;
using UnityEngine.UI;

namespace Lemmings.UI {

    /// <summary>
    /// Proxy for Unity's default slider.
    /// </summary>
    class SliderDesktop : SliderBase {

        /// <summary> The Unity slider attached to this object. </summary>
        private Slider slider;

        /// <summary> The value of the underlying slider. </summary>
        internal override float sliderValue {
            set { slider.value = value; }
        }

        /// <summary>
        /// Finds the Unity slider component.
        /// </summary>
        private void Start() {
            slider = GetComponent<Slider>();
        }
    }
}