using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    /*
        Find All Event Listners and Publish Message.
        Screen Lock
        Attend InputField Listener
        Virtual Keyboard Control
    */
    public class BCPG9_UIController : MonoBehaviour, IGameModule {
        [SerializeField] InputField answerInputField;
        [SerializeField] GameObject screenLock;

        private List<IUIEventCallback> eventCallbacks;
        private List<IUIUpdateCallback> updateCallbacks;
        private List<InputField> inputFields;

        public void Initialize(BCPG9GameSettings gameData, BCPG9_FourWord gameManager) {
            eventCallbacks = GetComponentsInChildren<IUIEventCallback>().ToList();
            updateCallbacks = GetComponentsInChildren<IUIUpdateCallback>().ToList();
            inputFields = GetComponentsInChildren<InputField>().ToList();
            answerInputField.onValueChanged.AddListener(OnInputValueChange);

            Debug.Log($"Event Callbacks:{eventCallbacks.Count}  Update Callbacks:{updateCallbacks.Count}");
        }

        public void ResetModule() {
        }

        public void CallEvent(BCPG9GameEventType eventType, BCPG9GameSettings gameData, BCPG9PlayData playData, string input = null) {
            eventCallbacks.ForEach(_ => _.OnEventCall(eventType, gameData, playData, input));
        }

        public void CallUpdate(BCPG9PlayData playData) {
            updateCallbacks.ForEach(_ => _.OnUpdateCall(playData));
        }

        public void LockInteraction(bool isLock) {
            screenLock.SetActive(isLock);
            if (isLock) {
                answerInputField.interactable = false;
            } else {
                answerInputField.interactable = true;
                answerInputField.ActivateInputField();
                answerInputField.text = "";
            }
        }

        public void SetKeyboard(bool isActive) {
            if (isActive)
                answerInputField.ActivateInputField();
            else
                answerInputField.DeactivateInputField();
        }

        private void OnInputValueChange(string input) {
            BCPG9_FourWord.CallInputEvent(answerInputField);
        }
    }
}