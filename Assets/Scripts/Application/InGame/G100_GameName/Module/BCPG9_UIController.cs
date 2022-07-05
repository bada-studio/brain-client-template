using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BCPG9 {
    /*
        Find All Event Listners and Publish Message.
        Screen Lock
        Attend InputField Listener
        Virtual Keyboard Control
    */
    public class BCPG9_UIController : MonoBehaviour, IGameModule {
        [SerializeField] CustomInputField answerInputField;
        [SerializeField] GameObject screenLock;

        private List<IUIEventCallback> eventCallbacks;
        private List<IUIUpdateCallback> updateCallbacks;
        private UnityEvent<string> inputEvent;
        private UnityEvent<string> editEndEvent;

        public void Initialize(BCPG9GameSettings gameData, BCPG9_FourWord gameManager) {
            eventCallbacks = GetComponentsInChildren<IUIEventCallback>().ToList();
            updateCallbacks = GetComponentsInChildren<IUIUpdateCallback>().ToList();
            inputEvent = new UnityEvent<string>();
            editEndEvent = new UnityEvent<string>();
            answerInputField.onValueChanged.AddListener(inputEvent.Invoke);
            answerInputField.onEditEnd.AddListener(editEndEvent.Invoke);

            Debug.Log($"Event Callbacks:{eventCallbacks.Count}  Update Callbacks:{updateCallbacks.Count}");
        }

        public void AddListener(UnityAction<string> inputAction, UnityAction<string> editEndAction) {
            inputEvent.AddListener(inputAction);
            editEndEvent.AddListener(editEndAction);
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
                answerInputField.SetInputEnable(false);
            } else {
                answerInputField.SetInputEnable(true);
                answerInputField.text = "";
            }
        }

        public void SetActiveKeyboard(bool isActive) {
            if (isActive)
                answerInputField.OpenKeyboard();
            else
                answerInputField.CloseKeyboard();
        }
    }
}