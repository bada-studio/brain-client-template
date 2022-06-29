/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author You sungHyun <1seconds@biscuitlabs.io>
* @created 2022/06/29
* @desc SceneControllerBase Template
*/
using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class SceneControllerBase : MonoBehaviour, SceneController {
    [SerializeField] private MyCanvasScaler canvasScaler;
    public abstract SceneType sceneType { get; }
    private static Stack<SceneType> sceneHistory = new Stack<SceneType>();

    protected bool InitController() {
        Service.scene = this;
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
        return true;
    }

    public virtual void OnSceneChanged(SceneType toScene) {
    }
    
    public async Task SwitchScene(SceneType scene) {
        Time.timeScale = 1;
        OnSceneChanged(scene);

        sceneHistory.Push(scene);
        SceneManager.LoadScene("Empty");
    }

    public static SceneType GetNextScene() {
        if (sceneHistory.Count == 0) {
            return SceneType.Intro;
        }
        
        return sceneHistory.Peek();
    }

    public async Task SwitchPrevScene() {
        if (sceneHistory.Count > 0) {
            sceneHistory.Pop();
            SceneManager.LoadScene("Empty");
        }
    }
}