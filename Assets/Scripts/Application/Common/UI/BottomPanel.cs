/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author You sungHyun <1seconds@biscuitlabs.io>
* @created 2022/06/30
* @desc BottomPanel Template
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnBottomPanelEvent(float height);

public class BottomPanel : ResultPopup {
    [SerializeField] private RectTransform anchor;
    [SerializeField] private RectTransform wrongAnchor2;
    [SerializeField] private float height = 100;

    public bool isShown { get; private set; }
    public event OnBottomPanelEvent onMoving;

    private void Awake() {
        HideImmediately();
        if (wrongAnchor2 != null) {
            HideImmediately2();
        }
    }

    private void HideImmediately() {
        var to = new Vector3(0, -height - 150, 0);
        anchor.anchoredPosition = to;
        anchor.gameObject.SetActive(false);
        isShown = false;
    }
    
    private void HideImmediately2() {
        var to = new Vector3(0, -height - 150, 0);
        wrongAnchor2.anchoredPosition = to;
        wrongAnchor2.gameObject.SetActive(false);
        isShown = false;
    }

    public void Show() {
        anchor.gameObject.SetActive(true);
        isShown = true;
        StartCoroutine(ShowRoutine());
    }
    
    public void Show2() {
        wrongAnchor2.gameObject.SetActive(true);
        isShown = true;
        StartCoroutine(ShowRoutine2());
    }

    public void Hide(bool easing) {
        isShown = false;
        if (easing == false || !this.gameObject.activeInHierarchy) {
            HideImmediately();
        } else {
            StartCoroutine(HideRoutine());
        }
    }
    
    public void Hide2(bool easing) {
        isShown = false;
        if (easing == false || !this.gameObject.activeInHierarchy) {
            HideImmediately2();
        } else {
            StartCoroutine(HideRoutine2());
        }
    }

    private IEnumerator ShowRoutine() {
        var to = Vector3.zero;
        Vector3 from = anchor.anchoredPosition;
        var easeType = EaseType.easeOutQuad;
        var duration = 0.3f;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            var progress = ease.Run();
            var pt = Vector3.Lerp(from, to, progress);
            anchor.anchoredPosition = pt;
            onMoving?.Invoke(height + pt.y);
            yield return null;
        }
    }
    
    private IEnumerator ShowRoutine2() {
        var to = Vector3.zero;
        Vector3 from = wrongAnchor2.anchoredPosition;
        var easeType = EaseType.easeOutQuad;
        var duration = 0.3f;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            var progress = ease.Run();
            var pt = Vector3.Lerp(from, to, progress);
            wrongAnchor2.anchoredPosition = pt;
            onMoving?.Invoke(height + pt.y);
            yield return null;
        }
    }

    private IEnumerator HideRoutine() {
        var to = new Vector3(0, -height - 150, 0);
        Vector3 from = anchor.anchoredPosition;
        var easeType = EaseType.easeOutQuad;
        var duration = 0.3f;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            var progress = ease.Run();
            var pt = Vector3.Lerp(from, to, progress);
            anchor.anchoredPosition = pt;
            onMoving?.Invoke(height + pt.y);
            yield return null;
        }
    
        anchor.gameObject.SetActive(false);
    }
    
    private IEnumerator HideRoutine2() {
        var to = new Vector3(0, -height - 150, 0);
        Vector3 from = wrongAnchor2.anchoredPosition;
        var easeType = EaseType.easeOutQuad;
        var duration = 0.3f;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            var progress = ease.Run();
            var pt = Vector3.Lerp(from, to, progress);
            wrongAnchor2.anchoredPosition = pt;
            onMoving?.Invoke(height + pt.y);
            yield return null;
        }
    
        wrongAnchor2.gameObject.SetActive(false);
    }
}
