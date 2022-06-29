using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public static class TweenExtensions {
    public static void PlayOnce(this MonoBehaviour v, Coroutine coroutine) {
        if (coroutine != null) {
            v.StopCoroutine(coroutine);
        }
    }

    public static IEnumerator MoveTo(this MonoBehaviour v, EaseType easeType, float duration, Vector3 to) {
        Vector3 from = v.transform.localPosition;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.transform.localPosition = Vector3.Lerp(from, to, ease.Run());
            yield return null;
        }
    }

    public static IEnumerator MoveTo(this Transform v, EaseType easeType, float duration, Vector3 to) {
        Vector3 from = v.transform.localPosition;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.transform.localPosition = Vector3.Lerp(from, to, ease.Run());
            yield return null;
        }
    }

    public static IEnumerator ScaleTo(this MonoBehaviour v, EaseType easeType, float duration, Vector3 to) {
        Vector3 from = v.transform.localScale;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.transform.localScale = Vector3.Lerp(from, to, ease.Run());
            yield return null;
        }
    }

    public static IEnumerator ScaleTo(this Transform v, EaseType easeType, float duration, Vector3 to) {
        Vector3 from = v.localScale;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.localScale = Vector3.Lerp(from, to, ease.Run());
            yield return null;
        }
    }
    
    public static IEnumerator ScaleTo(this RectTransform v, EaseType easeType, float duration, Vector2 to) {
        Vector2 from = v.sizeDelta;
        Vector3 localScale = v.localScale;
        if (localScale.x < 0) {
            from.x *= -1;
        }

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            var size = Vector2.Lerp(from, to, ease.Run());
            if (size.x < 0) {
                size.x *= -1;
                if (localScale.x > 0) {
                    localScale.x *= -1;
                    v.localScale = localScale;
                } 
            } else {
                if (localScale.x < 0) {
                    localScale.x *= -1;
                    v.localScale = localScale;
                }
            }

            v.sizeDelta = size;
            yield return null;
        }
    }    

    public static IEnumerator RotationTo(this MonoBehaviour v, EaseType easeType, float duration, Vector3 to) {
        Vector3 from = v.transform.localEulerAngles;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.transform.localEulerAngles = Vector3.Lerp(from, to, ease.Run());
            yield return null;
        }
    }

    public static IEnumerator MoveTo(this GameObject v, EaseType easeType, float duration, Vector3 to) {
        Vector3 from = v.transform.localPosition;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.transform.localPosition = Vector3.Lerp(from, to, ease.Run());
            yield return null;
        }
    }

    public static IEnumerator MoveTo(this RectTransform v, EaseType easeType, float duration, Vector3 to) {
        Vector3 from = v.anchoredPosition;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.anchoredPosition = Vector3.Lerp(from, to, ease.Run());
            yield return null;
        }
    }

    public static IEnumerator MoveTo(this RectTransform v, EaseType easeType, Vector2 toPos, float duration, Action<float> onEase, Action onDone) {
        var fromPos = v.anchoredPosition;
        yield return v.Ease(easeType, duration, (value) => {
            v.anchoredPosition = Vector2.Lerp(fromPos, toPos, value);
            onEase?.Invoke(value);
        });

        onDone?.Invoke();
    }

    public static IEnumerator Ease(this MonoBehaviour v, EaseType easeType, float duration, Action<float> onEase, Action onDone = null) {
        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            onEase(ease.Run());
            yield return null;
        }
        
        onDone?.Invoke();
    }

    public static IEnumerator Ease(this RectTransform v, EaseType easeType, float duration, Action<float> onEase, Action onDone = null) {
        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            onEase(ease.Run());
            yield return null;
        }
        
        onDone?.Invoke();
    }

    public static IEnumerator ScaleTo(this GameObject v, EaseType easeType, float duration, Vector3 to) {
        Vector3 from = v.transform.localScale;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.transform.localScale = Vector3.Lerp(from, to, ease.Run());
            yield return null;
        }
    }

    public static IEnumerator RotationTo(this GameObject v, EaseType easeType, float duration, Vector3 to) {
        Vector3 from = v.transform.localEulerAngles;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.transform.localEulerAngles = Vector3.Lerp(from, to, ease.Run());
            yield return null;
        }
    }

    public static IEnumerator ValueTo(this Slider v, EaseType easeType, float duration, float to) {
        float from = v.value;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.value = Mathf.Lerp(from, to, ease.Run());
            yield return null;
        }
    }

    public static IEnumerator CameraRectTo(this Camera v, EaseType easeType, float duration, Rect to) {
        Rect from = v.rect;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            Rect rect = new Rect();
            float process = ease.Run();
            rect.xMin = Mathf.Lerp(from.xMin, to.xMin, process);
            rect.yMin = Mathf.Lerp(from.yMin, to.yMin, process);
            rect.xMax = Mathf.Lerp(from.xMax, to.xMax, process);
            rect.yMax = Mathf.Lerp(from.yMax, to.yMax, process);
            v.rect = rect;
            yield return null;
        }
    }

    public static IEnumerator TintColorTo(this Material v, EaseType easeType, float duration, Color to) {
        Color from = v.GetColor("_TintColor");

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            Color color = Color.Lerp(from, to, ease.Run());
            v.SetColor("_TintColor", color);
            yield return null;
        }
    }

    public static IEnumerator AlphaTo(this CanvasGroup v, EaseType easeType, float duration, float to) {
        float from = v.alpha;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.alpha = Mathf.Lerp(from, to, ease.Run());
            yield return null;
        }
    }
    
    public static IEnumerator AlphaTo(this SpriteRenderer v, EaseType easeType, float duration, float to) {
        var fromColor = v.color;
        var toColor = v.color;
        toColor.a = to;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.color = Color.Lerp(fromColor, toColor, ease.Run());
            yield return null;
        }
    }
    
    public static IEnumerator ColorTo(this Graphic v, EaseType easeType, float duration, Color to) {
        Color from = v.color;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.color = Color.Lerp(from, to, ease.Run());
            yield return null;
        }
    }
    
    public static IEnumerator AlphaTo(this Graphic v, EaseType easeType, float duration, float to) {
        Color fromColor = v.color;
        Color toColor = v.color;
        toColor.a = to;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.color = Color.Lerp(fromColor, toColor, ease.Run());
            yield return null;
        }
    }

    public static IEnumerator WaveIt(this RectTransform v, int count, float power, float duration) {
        float waveTime = 0;
        Vector3 originPosition = v.anchoredPosition;
        
        while (waveTime < duration) {
            float process = waveTime / duration;
            float value = Mathf.Sin(2 * Mathf.PI * process * count);
            float radius = value * power;
            
            v.anchoredPosition = Vector3.up * radius + originPosition;
            waveTime += Time.deltaTime;
            yield return null;
        }

        v.anchoredPosition = originPosition;
    }

    
#if BOOT_NGUI_SUPPORT
    public static IEnumerator AlphaTo(this UIPanel v, EaseType easeType, float duration, float to) {
        float from = v.alpha;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.alpha = Mathf.Lerp(from, to, ease.Run());
#if UNITY_EDITOR
            NGUITools.SetDirty(v);
#endif
            yield return null;
        }
    }

    public static IEnumerator ColorTo(this UIWidget v, EaseType easeType, float duration, Color to) {
        Color from = v.color;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.color = Color.Lerp(from, to, ease.Run());
#if UNITY_EDITOR
            NGUITools.SetDirty(v);
#endif
            yield return null;
        }
    }

    public static IEnumerator AlphaTo(this UIWidget v, EaseType easeType, float duration, float to) {
        Color from = v.color;
        Color toClolor = v.color;
        toClolor.a = to;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            v.color = Color.Lerp(from, toClolor, ease.Run());
#if UNITY_EDITOR
            NGUITools.SetDirty(v);
#endif
            yield return null;
        }
    }
#endif

    public static IEnumerator TweenBounce(this MonoBehaviour v) {
        yield return v.ScaleTo(EaseType.easeOutQuad, 0.1f, Vector3.one * 0.9f);
        yield return v.ScaleTo(EaseType.easeInQuad, 0.1f, Vector3.one * 1.08f);
        yield return v.ScaleTo(EaseType.easeOutQuad, 0.1f, Vector3.one * 0.94f);
        yield return v.ScaleTo(EaseType.easeInQuad, 0.1f, Vector3.one * 1.02f);
        yield return v.ScaleTo(EaseType.easeOutQuad, 0.1f, Vector3.one * 1.0f);
    }
}