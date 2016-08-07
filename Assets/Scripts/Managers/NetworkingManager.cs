using UnityEngine;
using System;
using System.Collections;
using System.Net;
using Lemmings.Util.Timers;

namespace Lemmings.Managers {

    /// <summary> Processes a response received from the server. </summary>
    public delegate void HandleResponse(String response);

    /// <summary>
    /// Handles sending and receiving from the internet.
    /// </summary>
    class NetworkingManager : MonoBehaviour {

        /// <summary> The base URL of the server. </summary>
        [SerializeField]
        [Tooltip("The base URL of the server.")]
        private string baseURL;
        /// <summary> The URL for getting a level. </summary>
        [SerializeField]
        [Tooltip("The URL for getting a level.")]
        private string levelURL;
        /// <summary> The URL for checking whether to refresh the level. </summary>
        [SerializeField]
        [Tooltip("The URL for checking whether to refresh the level.")]
        private string dirtyURL;
        /// <summary> The URL for getting a refreshed level. </summary>
        [SerializeField]
        [Tooltip("The URL for getting a refreshed level.")]
        private string refreshURL;

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
        /// <param name="url">The URL to get a string from.</param>
        /// <param name="OnGet">The process to run on the string obtained from the URL.</param>
        /// <param name="form">The form to pass to the URL.</param>
        private void ProcessStringFromURL(string url, HandleResponse OnGet, WWWForm form = null) {
            StartCoroutine(GetURL(url, OnGet, form));
        }

        /// <summary>
        /// Gets a level from the server.
        /// </summary>
        /// <param name="OnGet">The process to run on the level JSON.</param>
        public void GetLevel(HandleResponse OnGet) {
            Settings settings = Settings.instance;
            settings.LoadSettings();

            WWWForm settingsForm = new WWWForm();
            settingsForm.AddField(Settings.PATH_KEY, settings.pathSetting.ToString());
            settingsForm.AddField(Settings.MOVEMENT_KEY, settings.movementSetting.ToString());
            settingsForm.AddField(Settings.DIFFICULTY_KEY, settings.difficultySetting.ToString());

            ProcessStringFromURL(levelURL, OnGet, settingsForm);
        }

        /// <summary>
        /// Checks if the level needs to be refreshed.
        /// </summary>
        /// <param name="OnGet">The process to run on the dirty response.</param>
        public void CheckDirty(HandleResponse OnGet) {
            ProcessStringFromURL(dirtyURL, OnGet);
        }

        /// <summary>
        /// Refreshes the level after being marked as dirty.
        /// </summary>
        /// <param name="OnGet">The process to run on the level refresh JSON.</param>
        public void RefreshLevel(HandleResponse OnGet, WWWForm form) {
            ProcessStringFromURL(refreshURL, OnGet, form);
        }

        /// <summary>
        /// Processes a string obtained from a URL.
        /// </summary>
        /// <param name="url">The URL to get a string from.</param>
        /// <param name="OnGet">The process to run on the string obtained from the specified URL.</param>
        /// <param name="form">The form to pass to the URL.</param>
        private IEnumerator GetURL(string url, HandleResponse OnGet, WWWForm form = null) {
            if (form == null) {
                form = new WWWForm();
            }
            WWW www = new WWW(baseURL + url, form);
            yield return www;

            if (www.error == null) {
                OnGet(www.text);
            } else {
                Debug.Log(www.error);
            }
        }
    }
}