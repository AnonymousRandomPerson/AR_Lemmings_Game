using UnityEngine;
using UnityEngine.UI;
using Lemmings.Entities.Player;
using Lemmings.Enums;
using Lemmings.Managers;

namespace Lemmings.UI {
    /// <summary>
    /// Displays details about blocks that can be placed.
    /// </summary>
    class BlockDisplay : MonoBehaviour {

        /// <summary> The block images in the UI. </summary>
        [SerializeField]
        [Tooltip("The block images in the UI.")]
        private GameObject[] blocks;
        /// <summary> The block counters next to the block images. </summary>
        private Text[] textCounters;
        /// <summary> The selector highlighting the currently selected block. </summary>
        [SerializeField]
        [Tooltip("The selector highlighting the currently selected block.")]
        private Transform selector;

        /// <summary> The block manager in the scene. </summary>
        private BlockManager blockManager;
        /// <summary> The player in the scene. </summary>
        private PlayerPlacer player;

        /// <summary>
        /// Finds needed objects in the scene.
        /// </summary>
        private void Start() {
            blockManager = BlockManager.instance;
            player = PlayerPlacer.instance;
            int numBlocks = blocks.Length;
            textCounters = new Text[numBlocks];
            for (int i = 0; i < numBlocks; i++) {
                textCounters[i] = blocks[i].GetComponentInChildren<Text>();
            }
        }
        
        /// <summary>
        /// Updates the block display.
        /// </summary>
        private void Update() {
            selector.position = blocks[(int)player.selectedBlock].transform.position;
            int numAvailable = 0;
            for (int i = 0; i < textCounters.Length; i++) {
                numAvailable = blockManager.GetNumAvailable((BlockType)i);
                textCounters[i].text = numAvailable.ToString();
                textCounters[i].color = numAvailable == 0 ? Color.red : Color.black;
            }
        }
    }
}