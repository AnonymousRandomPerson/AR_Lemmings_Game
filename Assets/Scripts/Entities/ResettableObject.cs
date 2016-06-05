using UnityEngine;
using Lemmings.Managers;

namespace Lemmings.Entities {
    /// <summary>
    /// An object that can be reset when the level restarts.
    /// </summary>
    public class ResettableObject : MonoBehaviour {

        /// <summary> The initial position of the object. </summary>
        private Vector3 initialPosition;
        /// <summary> The initial rotation of the object. </summary>
        private Quaternion initialRotation;

        /// <summary>
        /// Logs the initial conditions of the object.
        /// </summary>
        protected virtual void Start() {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            GameManager.instance.resetObjects.Add(this);
        }

        /// <summary>
        /// Resets the object.
        /// </summary>
        public virtual void Reset() {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }
}