using UnityEngine;
using UnityEngine.UI;
using Lemmings.Managers;

namespace Lemmings.UI {
    /// <summary>
    /// Displays the number of lemmings in the level.
    /// </summary>
    public class LemmingCounter : MonoBehaviour {

        /// <summary> The text to update. </summary>
        private Text text;

        /// <summary> The game manager to get the current time from. </summary>
        private GameManager gameManager;

        /// <summary>
        /// Finds needed objects in the scene.
        /// </summary>
        private void Start() {
            text = GetComponent<Text>();
            gameManager = GameManager.instance;
        }
    	
    	// Update is called once per frame
        private void Update() {
            int goalLemmings = gameManager.goalLemmings;
            int numLemmings = gameManager.numLemmings;
            text.text = goalLemmings + "/" + numLemmings;
            text.color = goalLemmings == numLemmings ? Color.green : Color.black;
    	}
    }
}