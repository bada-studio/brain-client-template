using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCPG9 {
    public class AnswerGrid : MonoBehaviour, IUIEventCallback {
        [SerializeField] List<AnswerCell> cellList;
        [SerializeField] private Color normalImgColor;
        [SerializeField] private Color correctImgColor;
        [SerializeField] private Color incorrectImgColor;
        [SerializeField] private Color normalTextColor;
        [SerializeField] private Color incorrectTextColor;

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameSettings gameData, BCPG9PlayData playData, string input = null) {
            switch (eventType) {
                case BCPG9GameEventType.NewQuiz:
                    UpdateAnswer(playData.rule);
                    UpdateInput(string.Empty);
                    UpdateColorOnNormal();
                    break;
                case BCPG9GameEventType.ResetInput:
                    UpdateColorOnNormal();
                    break;
                case BCPG9GameEventType.Input:
                    UpdateInput(input);
                    break;
                case BCPG9GameEventType.Correct:
                    UpdateColorOnCorrect();
                    break;
                case BCPG9GameEventType.Incorrect:
                    UpdateColorOnIncorrect();
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
        }

        private void UpdateColorOnNormal() {
            cellList.ForEach(_ => SetNormalColor(_));
        }
        private void UpdateColorOnCorrect() {
            SetNormalColor(cellList[0]);
            SetNormalColor(cellList[1]);
            SetCorrectColor(cellList[2]);
            SetCorrectColor(cellList[3]);
        }
        private void UpdateColorOnIncorrect() {
            SetNormalColor(cellList[0]);
            SetNormalColor(cellList[1]);
            SetIncorrectColor(cellList[2]);
            SetIncorrectColor(cellList[3]);
        }

        private void SetCorrectColor(AnswerCell cell) {
            cell.UpdateColor(correctImgColor, normalTextColor);
        }

        private void SetIncorrectColor(AnswerCell cell) {
            cell.UpdateColor(incorrectImgColor, incorrectTextColor);
        }

        private void SetNormalColor(AnswerCell cell) {
            cell.UpdateColor(normalImgColor, normalTextColor);
        }
    }
}