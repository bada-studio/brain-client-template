/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author You sungHyun <1seconds@biscuitlabs.io>
* @created 2022/06/30
* @desc ResultPopup Template
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Spine.Unity;

public class ResultPopup : MonoBehaviour {
    [SerializeField] private SkeletonGraphic graphic;
    [SerializeField] private Slider progressSlider;
    
    private Coroutine correctRoutine;
    private Coroutine wrongRoutine;
/*
    public void ResultRoutine(PlayLog log) {
        if (log.category != GameCategory.PlusSleep) {
            SoundManager.instance.PlaySFX("clear");
        }
        
        if (progressSlider != null) {
            StartCoroutine(ProgressSliderEasingRoutine((log.goal - 1),log.goal, log.goal));
        }

        log.progress += 1;
        Service.trainer.SaveCountData(log.category);
        Service.trainer.SaveToday();
        Service.trainer.SaveEData(log.category, 10);
        var data = Service.trainer.LoadSLData(log.category);
        Service.trainer.SaveSLData(log.category, data);
        Service.trainer.SaveSLToday(log.category, Service.trainer.GetDifficulty(log.category));
        GameSceneController.stopDoTime = true;
    }

    public void ProgressRoutine(PlayLog log) {
        if (log.category != GameCategory.PlusSleep) {
            SoundManager.instance.PlaySFX("clear2");
        }
        
        if (progressSlider != null) {
            StartCoroutine(ProgressSliderEasingRoutine((log.progress % log.goal),((log.progress + 1) % log.goal), log.goal));
        }
        
        log.progress += 1;
        Service.trainer.SaveToday();
    }
*/
    IEnumerator ProgressSliderEasingRoutine(int from, int to, int max) {
        progressSlider.maxValue = max;
        yield return this.Ease(EaseType.linear, 0.3f, (v) => {
            progressSlider.value =  Mathf.Lerp(from, to, v);
        });
    }

    public void OnCorrect() {
        graphic.gameObject.SetActive(true);
        graphic.Skeleton.SetSkin("skin1");
        graphic.Skeleton.SetSlotsToSetupPose();
        graphic.LateUpdate();
        
        graphic.AnimationState.SetAnimation(0, "start", false);
        if (correctRoutine != null) {
            StopCoroutine(correctRoutine);
        }
        correctRoutine = StartCoroutine(OnCorrectRoutine());
    }
    /*
    public void OnCorrect(GameCategory category) {
        graphic.gameObject.SetActive(true);
        graphic.Skeleton.SetSkin("skin1");
        graphic.Skeleton.SetSlotsToSetupPose();
        graphic.LateUpdate();
        
        graphic.AnimationState.SetAnimation(0, "start", false);
        if (correctRoutine != null) {
            StopCoroutine(correctRoutine);
        }
        correctRoutine = StartCoroutine(OnCorrectRoutine());
        //Service.trainer.SaveStageIndex(category, Service.trainer.GetDifficulty(category));
    }*/

    public void OnWrong() {
        graphic.gameObject.SetActive(true);
        graphic.Skeleton.SetSkin("skin2");
        graphic.Skeleton.SetSlotsToSetupPose();
        graphic.LateUpdate();
        
        graphic.AnimationState.SetAnimation(0, "start", false);
        //if (Service.setting.value.isVibrateOn) {
        //    Handheld.Vibrate();
        //}
        //SoundManager.instance.PlaySFX("wrong");
        if (wrongRoutine != null) {
            StopCoroutine(wrongRoutine);
        }
        wrongRoutine = StartCoroutine(OnWrongRoutine());
    }
    
    IEnumerator OnCorrectRoutine() {
        yield return new WaitForSeconds(1.5f);
        graphic.gameObject.SetActive(false);
    }

    IEnumerator OnWrongRoutine() {
        yield return new WaitForSeconds(1.5f);
        graphic.gameObject.SetActive(false);
    }

    public void OnWrongAndWait() {
        graphic.gameObject.SetActive(true);
        graphic.Skeleton.SetSkin("skin2");
        graphic.Skeleton.SetSlotsToSetupPose();
        graphic.LateUpdate();

        graphic.AnimationState.SetAnimation(0, "start", false);
        if (wrongRoutine != null) {
            StopCoroutine(wrongRoutine);
        }
        wrongRoutine = StartCoroutine(OnWrongRoutine());
    }
}
