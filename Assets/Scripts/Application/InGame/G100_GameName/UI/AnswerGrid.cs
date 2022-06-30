using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCPG9 {
    public class AnswerGrid : MonoBehaviour, IUIEventCallback {
        [SerializeField] List<AnswerCell> cellList;

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameData gameData, BCPG9PlayData playData) {
            switch (eventType) {
                case BCPG9GameEventType.NewQuiz:
                    UpdateAnswer(playData.rule);
                    break;
                case BCPG9GameEventType.Correct:
                    ActiveAnswer();
                    break;
            }
        }

        private void UpdateAnswer(BCPG9Rule answer) {
            cellList[0].SetShow(answer.word[0]);
            cellList[1].SetShow(answer.word[1]);
            cellList[2].SetHide(answer.word[2]);
            cellList[3].SetHide(answer.word[3]);
        }

        private void ActiveAnswer() {
            cellList[2].ShowHideAnswer();
            cellList[3].ShowHideAnswer();
        }
    }
}