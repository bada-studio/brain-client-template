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
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using CnControls;
using DG.Tweening;

namespace BCPG9 {
    /*기능설명*/
    public class BCPG9_FourWord : MonoBehaviour, ServiceStatePresenter {
        public static void CallGlobalEvent(BCPG9GameEventType eType) => globalEventCall?.Invoke(eType);
        public static void CallInputEvent(InputField field) => inputEventCall?.Invoke(field);
        private static UnityEvent<BCPG9GameEventType> globalEventCall;
        private static UnityEvent<InputField> inputEventCall;

        [SerializeField] BCPG9GameSettings gameData;
        [SerializeField] BCPG9_UIController uiController;
        [SerializeField] PopupController popupController;
        [SerializeField] Timer timer;
        [SerializeField] ScoreManager scoreManager;
        [SerializeField] Animator state;

        private List<IGameModule> modules;
        private RandomIndexProvider indexProvider;
        private Dictionary<int, BCPG9Rule> rules;
        private BCPG9PlayData playData;
        private bool isPaused = false;
        private bool isCloseEnd = false;
        private string currentInput;

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
                    CallGameEvent(BCPG9GameEventType.CloseEnd);
                }
                if (timer.isTimeExpired) {
                    state.SetTrigger("End");
                }
            }
        }

        #region Pause and Resume
        public void PauseGame() {
            uiController.LockInteraction(true);
            CallGameEvent(BCPG9GameEventType.Pause);
            isPaused = true;
        }

        public void ResumeGame() {
            CallGameEvent(BCPG9GameEventType.Resume);
            isPaused = false;
            uiController.LockInteraction(false);
        }

        public void ResetGame() {
            state.SetTrigger("Reset");
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
        #region Game Loop 
        private void Initialize() {
            globalEventCall = new UnityEvent<BCPG9GameEventType>();
            inputEventCall = new UnityEvent<InputField>();
            globalEventCall.AddListener(CallGameEvent);
            inputEventCall.AddListener(OnInputAnswer);

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
            uiController.gameObject.SetActive(true);
            indexProvider.ResetIndex();
            modules.ForEach(_ => _.ResetModule());
            UpdatePlayData();
            CallGameEvent(BCPG9GameEventType.Reset);
            isPaused = false;
            isCloseEnd = false;
            uiController.LockInteraction(false);
        }

        private void OnNewQuiz() {
            playData.rule = rules[indexProvider.GetIndex()];
            scoreManager.SetAnswer(playData.rule);
            CallGameEvent(BCPG9GameEventType.NewQuiz);
            uiController.OpenKeyboard();
        }

        private void OnIdle() {
            ResumeGame();
            CallGameEvent(BCPG9GameEventType.ResetInput);
        }

        private void OnCorrect() {
            PauseGame();
            CallGameEvent(BCPG9GameEventType.Correct);
            popupController.ShowResult(true, scoreManager.comboCount);
        }

        private void OnIncorrect() {
            PauseGame();
            CallGameEvent(BCPG9GameEventType.Incorrect);
            popupController.ShowResult(false, scoreManager.comboCount);
        }

        public void OnInputAnswer(InputField field) {
            currentInput = field.text;
            CallGameEvent(BCPG9GameEventType.Input);
            if (currentInput.Length >= 2) {
                var isCorrect = scoreManager.CheckAnswer(currentInput);
                if (!field.CheckKoreanInputEnd() && !isCorrect) {
                    Debug.Log(field.CheckKoreanInputEnd());
                    return;
                }
                state.SetTrigger(isCorrect ? "Correct" : "Incorrect");
            }
        }

        private void OnEnd() {
            PauseGame();
            popupController.ShowBottomPanel();
            CallGameEvent(BCPG9GameEventType.End);
        }
        #endregion

        private void UpdatePlayData() {
            playData.comboCount = scoreManager.comboCount;
            playData.score = scoreManager.currentScore;
            playData.remainTime = timer.remainTimeInt;
            playData.remainTimeRatio = timer.remainTime / gameData.limitedTime;
        }

        private void CallGameEvent(BCPG9GameEventType eventType) {
            switch (eventType) {
                case BCPG9GameEventType.HintOpen:
                    scoreManager.ClearCombo();
                    uiController.OpenKeyboard();
                    break;
                case BCPG9GameEventType.Pass:
                    scoreManager.PassAnswer();
                    OnNewQuiz();
                    break;
            }

            UpdatePlayData();
            uiController.CallEvent(eventType, gameData, playData, currentInput);
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