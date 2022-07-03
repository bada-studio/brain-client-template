using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    public class DescText : MonoBehaviour, IUIEventCallback {
        [SerializeField] private Text descText;

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameData gameData, BCPG9PlayData playData, string input = null) {
            switch (eventType) {
                case BCPG9GameEventType.NewQuiz:
                    descText.gameObject.SetActive(false);
                    break;
                case BCPG9GameEventType.HintOpen:
                    OpenDesc(PickRandomDesc(playData.rule));
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
    }
}