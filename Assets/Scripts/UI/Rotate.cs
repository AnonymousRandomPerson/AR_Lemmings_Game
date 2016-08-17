using UnityEngine;

namespace Lemmings.UI {
    /// <summary>
    /// Rotates an object.
    /// </summary>
    class Rotate : MonoBehaviour {

        /// <summary> The speed at which the object rotates. </summary>
        [SerializeField]
        [Tooltip("The speed at which the object rotates.")]
        private float rotateSpeed;

        /// <summary>
        /// Updates the object.
        /// </summary>
        private void Update() {
            transform.Rotate(new Vector3(0, 0, rotateSpeed));
        }
    }
}
