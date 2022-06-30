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
using CnControls;
using DG.Tweening;

namespace BCPG9 {
    /*기능설명*/
    public class BCPG9_FourWord : MonoBehaviour, ServiceStatePresenter {
        [SerializeField] InGameUIController uiController;
        [SerializeField] Timer timer;
        [SerializeField] ScoreManager scoreManager;

        private RuleIndexProvider indexProvider;
        private Dictionary<int, BCPG9Rule> rules;

        public async void Start() {
            Application.runInBackground = true;

            var isInit = await Boot();
            isInit = isInit && await uiController.Initilize(this, OnAnswer);
            if (!isInit)
                return;

            Initialize();
            ResetGame();
            SetQuiz();
        }

        #region Pause and Resume
        public void PauseGame() {
            uiController.PauseUI();
            timer.enabled = false;
        }

        public void ResumeGame() {

        }
        #endregion

        #region Game Flow
        private void Initialize() {
            rules = BCPG9_RuleService.instance.bcpg9Rule;
            indexProvider = new RuleIndexProvider(rules.Keys.ToList());
        }

        private void ResetGame() {
            indexProvider.ResetIndex();
            timer.ResetTimer();
            scoreManager.ResetScore();
            uiController.ResetUI(scoreManager.comboTime);
        }

        private void SetQuiz() {
            var picked = rules[indexProvider.GetIndex()];
            scoreManager.SetAnswer(picked);
            uiController.UpdateAnswerUI(picked, timer.time);
        }

        private void OnAnswer(string answer) {

        }

        private void GameEnd() {

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