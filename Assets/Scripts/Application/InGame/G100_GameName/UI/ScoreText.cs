using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    /*
        Header Score Text Component Event Handler
    */
    public class ScoreText : MonoBehaviour, IUIEventCallback {
        [SerializeField] private Text scoreText;

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameSettings gameData, BCPG9PlayData playData, string input = null) {
            switch (eventType) {
                case BCPG9GameEventType.Reset:
                    scoreText.text = "0";
                    break;
                case BCPG9GameEventType.Correct:
                case BCPG9GameEventType.NewQuiz:
                    scoreText.text = playData.score.ToString();
                    break;
            }
        }
    }
}