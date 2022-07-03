﻿/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author You sungHyun <1seconds@biscuitlabs.io>
* @created 2022/06/29
* @desc IntroSceneController Template
*/
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IntroSceneController : SceneControllerBase, ServiceStatePresenter {
    public override SceneType sceneType => SceneType.Intro;

    public async void Start() {
        Application.runInBackground = true;

        if (await Boot() == false) {
            SwitchScene(0).RunAsync();
            return;
        }
        
        //mainScene
        SwitchScene(SceneType.G200_GameName).RunAsync();
    }

    public void ShowServiceState(string key) {
    }

    private async Task<bool> Boot() {
        List<IService> services = new List<IService>();
        services.Add(Service.rule);
        services.Add(Service.lottoRule);
        services.Add(Service.userData);
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
}
