using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    /*
        Answer Description Text Component Event Handler
    */
    public class DescText : MonoBehaviour, IUIEventCallback {
        [SerializeField] private Text descText;

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameSettings gameData, BCPG9PlayData playData, string input = null) {
            switch (eventType) {
                case BCPG9GameEventType.NewQuiz:
                    descText.gameObject.SetActive(false);
                    break;
                case BCPG9GameEventType.HintOpen:
                    OpenDesc(PickFixedDesc(playData.rule));
                    break;
                case BCPG9GameEventType.Correct:
                    OpenDesc(playData.rule.answer[input]);
                    break;
            }
        }

        private void OpenDesc(string desc) {
            descText.text = desc;
            descText.gameObject.SetActive(true);
        }

        private static string PickRandomDesc(BCPG9Rule rule) {
            var answerDict = rule.answer;
            var randomIndex = Random.Range(0, answerDict.Count);
            return answerDict.ElementAt(randomIndex).Value;
        }

        private static string PickFixedDesc(BCPG9Rule rule) {
            return rule.answer[rule.hint];
        }
    }
}