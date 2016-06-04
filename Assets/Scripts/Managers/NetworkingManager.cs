using UnityEngine;
using System;
using System.Collections;
using System.Net;

namespace Lemmings.Managers {
    /// <summary>
    /// Handles sending and receiving from the internet.
    /// </summary>
    class NetworkingManager : MonoBehaviour {

        /// <summary> Where to send and receive data from. </summary>
        [Tooltip("Where to send and receive data from.")]
        public string url;

        /// <summary> The singleton instance of the networking manager. </summary>
        public static NetworkingManager instance;

        /// <summary>
        /// Sets the singleton instance.
        /// </summary>
        private void Awake() {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Processes a string obtained from a URL.
        /// </summary>
        /// <param name="OnGet">The process to run on the string obtained from the URL.</param>
        public void ProcessStringFromURL(Action<string> OnGet) {
            StartCoroutine(GetURL(OnGet));
        }

        /// <summary>
        /// Processes a string obtained from a URL.
        /// </summary>
        /// <param name="OnGet">The process to run on the string obtained from the specified URL.</param>
        private IEnumerator GetURL(Action<string> OnGet) {
            WWWForm form = new WWWForm();
            WWW www = new WWW(url, form);
            yield return www;

            if (www.error == null) {
                OnGet(www.text);
            } else {
                Debug.Log(www.error);
            }
        }
    }
}