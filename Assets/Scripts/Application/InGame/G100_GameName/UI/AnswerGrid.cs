using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCPG9 {
    public class AnswerGrid : MonoBehaviour, IUIEventCallback {
        [SerializeField] List<AnswerCell> cellList;

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameData gameData, BCPG9PlayData playData, string input = null) {
            switch (eventType) {
                case BCPG9GameEventType.Reset:
                    ChangeTextColor(false);
                    break;
                case BCPG9GameEventType.NewQuiz:
                    UpdateAnswer(playData.rule);
                    UpdateInput(string.Empty);
                    break;
                case BCPG9GameEventType.Input:
                    UpdateInput(input);
                    break;
                case BCPG9GameEventType.Correct:
                    ShowCorrect();
                    break;
                case BCPG9GameEventType.Incorrect:
                    ShowIncorrect();
                    break;
                case BCPG9GameEventType.CloseEnd:
                    ChangeTextColor(true);
                    break;
            }
        }

        private void UpdateAnswer(BCPG9Rule answer) {
            cellList[0].UpdateCharacter(answer.word[0]);
            cellList[1].UpdateCharacter(answer.word[1]);
        }

        private void UpdateInput(string input) {
            cellList[2].UpdateCharacter(input.Length > 0 ? input[0] : ' ');
            cellList[3].UpdateCharacter(input.Length > 1 ? input[1] : ' ');
            cellList[2].ChangeTextColor(false);
            cellList[3].ChangeTextColor(false);
        }

        private void ShowCorrect() {
            cellList[2].ShowCorrectUI();
            cellList[3].ShowCorrectUI();
        }

        private void ShowIncorrect() {
            cellList[2].ShowIncorrectUI();
            cellList[3].ShowIncorrectUI();
        }

        private void ChangeTextColor(bool isAlert) {
            cellList[0].ChangeTextColor(isAlert);
            cellList[1].ChangeTextColor(isAlert);
        }
    }
}