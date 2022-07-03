using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace BCPG9 {
    public class InGameUIController : MonoBehaviour, IGameModule {
        [SerializeField] InputField answerInputField;
        [SerializeField] ResultPopup resultPopup;
        [SerializeField] BottomPanel bottomPanel;
        [SerializeField] GameObject screenLock;
        [SerializeField] ComboCounter comboCounter;

        private List<IUIEventCallback> eventCallbacks;
        private List<IUIUpdateCallback> updateCallbacks;

        public void Initialize(BCPG9GameData gameData, BCPG9_FourWord gameManager) {
            eventCallbacks = GetComponentsInChildren<IUIEventCallback>().ToList();
            updateCallbacks = GetComponentsInChildren<IUIUpdateCallback>().ToList();
            answerInputField.onValueChanged.AddListener(OnInputValueChange);
        }

        public void ResetModule() {
        }

        public void CallEvent(BCPG9GameEventType eventType, BCPG9GameData gameData, BCPG9PlayData playData, string input = null) {
            eventCallbacks.ForEach(_ => _.OnEventCall(eventType, gameData, playData, input));
        }

        public void CallUpdate(BCPG9PlayData playData) {
            updateCallbacks.ForEach(_ => _.OnUpdateCall(playData));
        }

        public void ShowResult(bool isCorrect, int comboCount) {
            if (isCorrect) {
                resultPopup.OnCorrect();
                comboCounter.ShowCombo(comboCount);
            } else {
                resultPopup.OnWrong();
            }
        }

        public void ShowBottomPanel() {
            bottomPanel.Show();
        }

        public void LockInteraction(bool isLock) {
            screenLock.SetActive(isLock);
            answerInputField.interactable = !isLock;
            if (!isLock) {
                answerInputField.ActivateInputField();
                answerInputField.text = "";
            }
        }

        public void OpenKeyboard() {
            answerInputField.ActivateInputField();
        }

        private void OnInputValueChange(string input) {
            BCPG9_FourWord.CallInputEvent(answerInputField);
        }
    }
}