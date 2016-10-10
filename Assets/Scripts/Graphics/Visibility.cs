using UnityEngine;
using Lemmings.Managers;
using Lemmings.Util;
using Lemmings.Util.Timers;

namespace Lemmings.Graphics {
    /// <summary>
    /// Handles visibility settings.
    /// </summary>
    class Visibility : MonoBehaviour {

        /// <summary> The mesh to toggle visibility with. </summary>
        [SerializeField]
        [Tooltip("The mesh to toggle visibility with.")]
        private GameObject mesh;
        /// <summary> Whether or not the environment mesh is visible. </summary>
        private bool meshVisible;

        /// <summary> Timer to prevent input from occurring too fast. </summary>
        private LimitTimerCallback keyTimer;
        /// <summary> The time to wait before the visibility key can be pressed again. </summary>
        [SerializeField]
        [Tooltip("The time to wait before the visibility key can be pressed again.")]
        private float keyCooldown;

        /// <summary> Possible visibility settings. </summary>
        private enum Settings {Mesh = 1, Surface, Both};
        /// <summary> The current visibility setting. </summary>
        private Settings currentSetting = Settings.Mesh;

        /// <summary> The surface manager in the scene. </summary>
        private SurfaceManager surfaceManager;

        /// <summary>
        /// Initializes the key timer.
        /// </summary>
        private void Start() {
            keyTimer = new LimitTimerCallback(ChangeSetting, keyCooldown);
            surfaceManager = SurfaceManager.instance;
        }

        /// <summary>
        /// Checks for input toggling visibility settings.
        /// </summary>
        private void Update() {
            if (InputUtil.GetKey(KeyCode.Tab)) {
                keyTimer.Run();
            }
        }

        /// <summary>
        /// Changes the visibility setting to the next one.
        /// </summary>
        private void ChangeSetting() {
            int newSetting = (int)currentSetting + 1;
            if (newSetting > 3) {
                newSetting = 1;
            }
            currentSetting = (Settings)newSetting;

            ApplySetting();
        }

        /// <summary>
        /// Changes the visibility of the mesh to the current setting.
        /// </summary>
        internal void ApplySetting() {
            int settingInt = (int)currentSetting;
            if (mesh != null) {
                mesh.SetActive((settingInt & 1) > 0);
            }
            surfaceManager.SetVisible((settingInt & 2) > 0);
        }
    }
}