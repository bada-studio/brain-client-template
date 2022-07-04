using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCPG9 {
    /*
        Control Middle Button Group {Help, Hint, Pass}
    */
    public class ButtonGroup : MonoBehaviour, IUIEventCallback {
        [SerializeField] private GameObject infoButton;
        [SerializeField] private GameObject hintButton;
        [SerializeField] private GameObject nextButton;

        private void OnNewQuiz() {
            hintButton.SetActive(true);
            nextButton.SetActive(false);
        }

        private void OnHint() {
            hintButton.SetActive(false);
            nextButton.SetActive(true);
        }

        #warning Open Game Information Button Event Listner
        public void OnClickInfoButton() {

        }

        public void OnClickPassButton() {
            BCPG9_FourWord.CallGlobalEvent(BCPG9GameEventType.Pass);
        }

        public void OnClickHintButton() {
            OnHint();
            BCPG9_FourWord.CallGlobalEvent(BCPG9GameEventType.HintOpen);
        }

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameSettings gameData, BCPG9PlayData playData, string input = null) {
            switch (eventType) {
                case BCPG9GameEventType.NewQuiz:
                    OnNewQuiz();
                    break;
            }
        }
    }
}