using UnityEngine;

namespace Lemmings.Util {
    /// <summary>
    /// Utility methods for object manipulation.
    /// </summary>
    static class ObjectUtil {

        /// <summary>
        /// Creates a new empty game object.
        /// </summary>
        /// <returns>The new empty game object.</returns>
        /// <param name="name">The name of the game object.</param>
        /// <param name="parent">The parent object of the game object, or null to place the game object at the root level.</param>
        public static GameObject CreateNewObject(string name, GameObject parent = null) {
            GameObject newObject = new GameObject();
            newObject.name = name;
            if (parent != null) {
                newObject.transform.parent = parent.transform;
            }
            return newObject;
        }

        /// <summary>
        /// Instantiates a game object, removing "(Clone)" from its name.
        /// </summary>
        /// <param name="original">The game object to instantiate.</param>
        public static T Instantiate<T>(T original) where T : Object {
            T newObject = Object.Instantiate(original);
            Object gameObject = (Object) newObject;
            gameObject.name = RemoveClone(gameObject.name);
            return newObject;
        }

        /// <summary>
        /// Instantiates a game object, removing "(Clone)" from its name.
        /// </summary>
        /// <param name="original">The game object to instantiate.</param>
        public static Object Instantiate(Object original) {
            return Instantiate(original, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Instantiates a game object, removing "(Clone)" from its name.
        /// </summary>
        /// <param name="original">The object to instantiate.</param></param>
        /// <param name="position">The initial position of the object.</param>
        public static Object Instantiate(Object original, Vector3 position) {
            return Instantiate(original, position, Quaternion.identity);
        }

        /// <summary>
        /// Instantiates a game object, removing "(Clone)" from its name.
        /// </summary>
        /// <param name="original">The object to instantiate.</param></param>
        /// <param name="position">The initial position of the object.</param>
        /// <param name="rotation">The initial rotation of the object.</param>
        public static Object Instantiate(Object original, Vector3 position, Vector3 rotation) {
            return Instantiate(original, position, Quaternion.Euler(rotation));
        }

        /// <summary>
        /// Instantiates a game object, removing "(Clone)" from its name.
        /// </summary>
        /// <param name="original">The object to instantiate.</param></param>
        /// <param name="position">The initial position of the object.</param>
        /// <param name="rotation">The initial rotation of the object.</param>
        public static Object Instantiate(Object original, Vector3 position, Quaternion rotation) {
            Object newObject = Object.Instantiate(original, position, rotation);
            newObject.name = RemoveClone(newObject.name);
            return newObject;
        }

        /// <summary>
        /// Removes " (Clone)" from a spawned game object.
        /// </summary>
        /// <returns>The name of the game object with " (Clone)" removed.</returns>
        /// <param name="name">The name of the game object.</param>
        private static string RemoveClone(string name) {
            return name.Substring(0, name.Length - 7);
        }
    }
}

