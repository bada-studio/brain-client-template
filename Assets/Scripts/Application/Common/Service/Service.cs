/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author You sungHyun <1seconds@biscuitlabs.io>
* @created 2022/06/29
* @desc Service Template
*/
using System.Threading.Tasks;
using UnityEngine;

public class Service : SingletonGameObject<Service> {
    // Services
    //-------------------------------------------------------------------------
    public static SceneController scene { get; set; }
    public static RuleService rule => RuleService.instance;
    
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        
        InitLogHandler();
    }

    private void InitLogHandler() {
        Debug.Log("SERVICE STARTED!");
    }
}

public static class AsyncRunner { 
    public static async void RunAsync(this Task task) {
        await task;
    }
}
