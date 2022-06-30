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
        [SerializeField] private int maxRoundCount;
        [SerializeField] private float limitTime;
        [SerializeField] private int standardScore;
        [SerializeField] private float comboCheckTime;
        [SerializeField] private int maxComboCount;
        [SerializeField] private float comboMultiplier;
        [SerializeField] private float passMultiplier;
        [SerializeField] Timer timer;

        private RuleIndexProvider indexProvider;
        private ComboCounter comboCounter;

        public async void Start() {
            Application.runInBackground = true;

            if (await Boot() == false) {
                return;
            }

            Initialize();
            ResetGame();
            UpdateUI();
        }

        private void Initialize() {
            var rules = BCPG9_RuleService.instance.bcpg9Rule;
            indexProvider = new RuleIndexProvider(rules.Keys.ToList());
            comboCounter = new ComboCounter(comboCheckTime, timer);
        }

        private void ResetGame() {
            indexProvider.ResetIndex();
            comboCounter.ResetCounter();
        }

        private void UpdateUI() {
            
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