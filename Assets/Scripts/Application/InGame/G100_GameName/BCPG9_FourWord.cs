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
        private static UnityEvent<BCPG9GameEventType> globalEventCall;

        [SerializeField] BCPG9GameData gameData;
        [SerializeField] InGameUIController uiController;
        [SerializeField] Timer timer;
        [SerializeField] ScoreManager scoreManager;

        private List<IGameModule> modules;
        private RuleIndexProvider indexProvider;
        private Dictionary<int, BCPG9Rule> rules;
        private BCPG9PlayData playData;
        private bool isPaused = false;

        public async void Start() {
            var isInit = await Boot();
            if (!isInit)
                return;
            Initialize();
            ResetGame();
        }

        private void Update() {
            if (!isPaused) {
                timer.UpdateTimer();
                UpdatePlayData();
                uiController.CallUpdate(playData);
                if (timer.isTimeExpired)
                    GameEnd();
            }
        }

        private void UpdatePlayData() {
            playData.comboCount = scoreManager.comboCount;
            playData.score = scoreManager.currentScore;
            playData.limitedTime = timer.limitedTimeInt;
            playData.remainComboTime = scoreManager.remainComboTime;
            playData.remainComboRatio = playData.remainComboTime / gameData.comboCheckTime;
        }

        private void CallGameEvent(BCPG9GameEventType eventType) {
            switch (eventType) {
                case BCPG9GameEventType.Pass:
                    scoreManager.PassAnswer();
                    SetQuiz();
                    break;
            }

            UpdatePlayData();
            uiController.CallEvent(eventType, gameData, playData);
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
        #endregion

        /*
            Initilaize -> Reset -> SetQuiz -> OnAnswer
                                     ┕-----------┙
        */
        #region Game Loop 
        private void Initialize() {
            globalEventCall = new UnityEvent<BCPG9GameEventType>();
            globalEventCall.AddListener(CallGameEvent);

            rules = BCPG9_RuleService.instance.bcpg9Rule;
            indexProvider = new RuleIndexProvider(rules.Keys.ToList());
            playData = new BCPG9PlayData();

            modules = new List<IGameModule>();
            modules.Add(scoreManager);
            modules.Add(timer);
            modules.Add(uiController);
            modules.ForEach(_ => _.Initialize(gameData, this));
        }

        public void ResetGame() {
            indexProvider.ResetIndex();
            modules.ForEach(_ => _.ResetModule());
            UpdatePlayData();
            CallGameEvent(BCPG9GameEventType.Reset);
            isPaused = false;
            uiController.LockInteraction(false);
            SetQuiz();
        }

        private void SetQuiz() {
            playData.rule = rules[indexProvider.GetIndex()];
            scoreManager.SetAnswer(playData.rule);
            CallGameEvent(BCPG9GameEventType.NewQuiz);
        }

        public void OnAnswer(string answer) {
            if (answer.Length >= 2) {
                var isNotEnd = scoreManager.CheckNotEnd(answer);
                var isCorrect = scoreManager.CheckAnswer(answer);
                if (isNotEnd && !isCorrect)
                    return;
                StartCoroutine(AnswerWaitRoutine(isCorrect));
            }
        }

        IEnumerator AnswerWaitRoutine(bool isCorrect) {
            if (isCorrect)
                CallGameEvent(BCPG9GameEventType.Correct);
            else
                CallGameEvent(BCPG9GameEventType.Incorrect);

            var animWait = new WaitForSeconds(1.333f);
            PauseGame();
            uiController.ShowResult(isCorrect, scoreManager.comboCount);
            yield return animWait;

            if (isCorrect)
                SetQuiz();
            ResumeGame();

            yield return null;
        }

        private void GameEnd() {
            PauseGame();
            uiController.ShowBottomPanel();
        }
        #endregion

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