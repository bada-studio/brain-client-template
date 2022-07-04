/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author Ha Sungmin <sm.ha@biscuitlabs.io>
* @created 2022/06/27
* @desc G100_GameName Template
*/
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BCPG9 {
    /*
        Game Start Point
        Object Initialize -> Fsm transition,UI Controller Event Sending -> Game End 
    */
    public class BCPG9_FourWord : MonoBehaviour, ServiceStatePresenter {
        #region Event Binder
        public static void CallGlobalEvent(BCPG9GameEventType eType) => globalEventCall?.Invoke(eType);
        public static void CallInputEvent(InputField field) => inputEventCall?.Invoke(field);
        private static UnityEvent<BCPG9GameEventType> globalEventCall;
        private static UnityEvent<InputField> inputEventCall;
        #endregion

        #region Serialize Field
        [SerializeField] BCPG9GameSettings gameData;
        [SerializeField] BCPG9_UIController uiController;
        [SerializeField] PopupController popupController;
        [SerializeField] Timer timer;
        [SerializeField] ScoreManager scoreManager;
        [SerializeField] Animator state;
        #endregion

        #region Private Field
        private List<IGameModule> modules;
        private RandomIndexProvider indexProvider;
        private Dictionary<int, BCPG9Rule> rules;
        private BCPG9PlayData playData;
        private bool isPaused = false;
        private bool isCloseEnd = false;
        private string currentInput;
        #endregion

        #region MonoBehaviour
        public async void Start() {
            var isInit = await Boot();
            if (!isInit)
                return;
            Initialize();
        }

        private void Update() {
            if (!isPaused) {
                timer.UpdateTimer();
                UpdatePlayData();
                uiController.CallUpdate(playData);
                if (timer.remainTime <= 10 && !isCloseEnd) {
                    isCloseEnd = true;
                    CallGlobalEvent(BCPG9GameEventType.CloseEnd);
                }
                if (timer.isTimeExpired) {
                    state.SetTrigger("End");
                }
            }
        }
        #endregion

        #region Public Method
        // Go to game start state
        public void ResetGame() {
            state.SetTrigger("Reset");
        }

        public void PauseGame() {
            isPaused = true;
            uiController.LockInteraction(true);
            CallGlobalEvent(BCPG9GameEventType.Pause);
        }

        public void ResumeGame() {
            isPaused = false;
            CallGlobalEvent(BCPG9GameEventType.Resume);
            uiController.LockInteraction(false);
        }
        #endregion

        /*
            <Trigger List>
            InitEnd: 초기화 끝난 후
            Correct: 정답 시
            Incorrect: 오답 시
            End: 타이머 끝날 시
            Reset: 게임 초기화 시
            Quit: 게임 종료
        */
        #region FSM Actions 
        private void Initialize() {
            Debug.Log("Initialize Start");
            globalEventCall ??= new UnityEvent<BCPG9GameEventType>();
            inputEventCall ??= new UnityEvent<InputField>();

            rules = BCPG9_RuleService.instance.bcpg9Rule;
            indexProvider = new RandomIndexProvider(rules.Keys.ToList());
            playData = new BCPG9PlayData();

            modules = new List<IGameModule>();
            modules.Add(scoreManager);
            modules.Add(timer);
            modules.Add(uiController);
            modules.ForEach(_ => _.Initialize(gameData, this));
            uiController.gameObject.SetActive(false);
            state.SetTrigger("InitEnd");
        }

        private void OnReset() {
            globalEventCall.AddListener(CallGameEvent);
            inputEventCall.AddListener(OnInputAnswer);
            
            Debug.Log("Reset State");
            uiController.gameObject.SetActive(true);
            indexProvider.ResetIndex();
            modules.ForEach(_ => _.ResetModule());
            UpdatePlayData();
            CallGlobalEvent(BCPG9GameEventType.Reset);
            isPaused = false;
            isCloseEnd = false;
            uiController.LockInteraction(false);
        }

        private void OnNewQuiz() {
            Debug.Log("NewQuiz State");
            playData.rule = rules[indexProvider.GetIndex()];
            scoreManager.SetAnswer(playData.rule);
            CallGlobalEvent(BCPG9GameEventType.NewQuiz);
            uiController.SetKeyboard(true);
        }

        private void OnIdle() {
            Debug.Log("Idle State");
            ResumeGame();
            CallGlobalEvent(BCPG9GameEventType.ResetInput);
        }

        private void OnCorrect() {
            Debug.Log("Correct State");
            PauseGame();
            CallGlobalEvent(BCPG9GameEventType.Correct);
            popupController.ShowResult(true, scoreManager.comboCount);
        }

        private void OnIncorrect() {
            Debug.Log("Incorrect State");
            PauseGame();
            CallGlobalEvent(BCPG9GameEventType.Incorrect);
            popupController.ShowResult(false, scoreManager.comboCount);
        }
        private void OnEnd() {
            Debug.Log("End State");
            uiController.SetKeyboard(false);
            PauseGame();
            popupController.ShowBottomPanel();
            CallGlobalEvent(BCPG9GameEventType.End);

            globalEventCall.RemoveAllListeners();
            inputEventCall.RemoveAllListeners();
        }
        #endregion

        #region Event Handler
        // Handle Input Event
        private void OnInputAnswer(InputField field) {
            currentInput = field.text;
            CallGlobalEvent(BCPG9GameEventType.Input);
            if (currentInput.Length >= 2) {
                var isCorrect = scoreManager.CheckAnswer(currentInput);
                if (!field.CheckKoreanInputEnd() && !isCorrect) {
                    Debug.Log(field.CheckKoreanInputEnd());
                    return;
                }
                state.SetTrigger(isCorrect ? "Correct" : "Incorrect");
            }
        }

        // Handle Global Sending Event
        private void CallGameEvent(BCPG9GameEventType eventType) {
            switch (eventType) {
                case BCPG9GameEventType.HintOpen:
                    scoreManager.ClearCombo();
                    uiController.SetKeyboard(true);
                    break;
                case BCPG9GameEventType.Pass:
                    scoreManager.PassAnswer();
                    OnNewQuiz();
                    break;
            }

            UpdatePlayData();
            uiController.CallEvent(eventType, gameData, playData, currentInput);
        }
        #endregion

        private void UpdatePlayData() {
            playData.comboCount = scoreManager.comboCount;
            playData.score = scoreManager.currentScore;
            playData.remainTime = timer.remainTimeInt;
            playData.remainTimeRatio = timer.remainTime / gameData.limitedTime;
        }

        #region IntroSceneController.cs Copy
        public void ShowServiceState(string key) {
        }

        private async Task<bool> Boot() {
            List<IService> services = new List<IService>();
            services.Add(BCPG9_RuleService.instance);
            return await InitializeServices(services);
        }

        private async Task<bool> InitializeServices(List<IService> services) {
            foreach (IService service in services) {
                bool result = await service.Initialize(this);
                if (!result) {
                    Debug.LogError(service.type + "service load failed");
                    return false;
                } else {
                    Debug.Log(service.type + "service ready");
                }
            }

            return true;
        }
        #endregion
    }
}