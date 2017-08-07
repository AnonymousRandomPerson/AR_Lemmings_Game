using UnityEngine;
using Lemmings.Enums

namespace Lemmings.Managers {
    /// <summary>
    /// 
    /// </summary>
    class Settings : MonoBehaviour {

        /// <summary> The singleton settings instance. </summary>
        private static Settings _instance;
        /// <summary> The singleton settings instance. </summary>
        public static Settings instance {
            get { return _instance; }
        }

        /// <summary> The length of the level path. </summary>
        private float _pathSetting = 0.5f;
        /// <summary> The length of the level path. </summary>
        public float pathSetting {
            get { return _pathSetting; }
            set { 
                _pathSetting = value;
                SetSetting(PlayerPrefsKey.Path, value);
            }
        }
        /// <summary> The amount of human movement in the level. </summary>
        private float _movementSetting = 0.5f;
        /// <summary> The amount of human movement in the level. </summary>
        public float movementSetting {
            get { return _movementSetting; }
            set { 
                _movementSetting = value;
                SetSetting(PlayerPrefsKey.Movement, value);
            }
        }
        /// <summary> The difficulty of the level. </summary>
        private float _difficultySetting = 0.5f;
        /// <summary> The difficulty of the level. </summary>
        public float difficultySetting {
            get { return _difficultySetting; }
            set { 
                _difficultySetting = value;
                SetSetting(PlayerPrefsKey.Difficulty, value);
            }
        }

        /// <summary>
        /// Sets the settings instance.
        /// </summary>
        private void Awake() {
            _instance = this;
        }

        /// <summary>
        /// Loads existing settings.
        /// </summary>
        public void LoadSettings() {
            LoadSetting(PlayerPrefsKey.Path, ref _pathSetting);
            LoadSetting(PlayerPrefsKey.Movement, ref _movementSetting);
            LoadSetting(PlayerPrefsKey.Difficulty, ref _difficultySetting);
        }

        /// <summary>
        /// Loads a setting if it exists.
        /// </summary>
        /// <param name="key">The key of the setting.</param>
        /// <param name="setting">The setting variable to set.</param>
        private void LoadSetting(PlayerPrefsKey key, ref float setting) {
            string keyString = GetKeyString(key);
            if (PlayerPrefs.HasKey(keyString)) {
                setting = PlayerPrefs.GetFloat(keyString);
            }
        }

        /// <summary>
        /// Sets the value of a PlayerPrefs field to a float.
        /// </summary>
        /// <param name="key">The key of the field to set.</param>
        /// <param name="floatValue">The float value to set the key to.</param>
        private void SetSetting(PlayerPrefsKey key, float floatValue) {
            PlayerPrefs.SetFloat(GetKeyString(key), floatValue);
        }

        /// <summary>
        /// Returns the string form of a PlayerPrefsKey enum.
        /// </summary>
        /// <returns>The string form of the given PlayerPrefsKey enum.</returns>
        /// <param name="key">The PlayerPrefsKey enum to convert.</param>
        public static string GetKeyString(PlayerPrefsKey key) {
            return key.ToString().ToLower();
        }
    }
}
