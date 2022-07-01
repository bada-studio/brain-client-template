using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    public class DescText : MonoBehaviour, IUIEventCallback {
        [SerializeField] private Text descText;

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameData gameData, BCPG9PlayData playData) {
            switch (eventType) {
                case BCPG9GameEventType.NewQuiz:
                    descText.gameObject.SetActive(false);
                    descText.text = playData.rule.desc;
                    break;
                case BCPG9GameEventType.HintOpen:
                case BCPG9GameEventType.Correct:
                    descText.gameObject.SetActive(true);
                    break;
            }
        }
    }
}