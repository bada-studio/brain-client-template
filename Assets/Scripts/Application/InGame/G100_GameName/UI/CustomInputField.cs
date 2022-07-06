using System;
using UnityEngine;
using UnityEngine.Events;

public class CustomInputField : MonoBehaviour {
    [SerializeField] private string placeHolderText;

    public string text {
        get {
            return currentKeyboard != null ? currentKeyboard.text : null;
        }
        set {
            if (currentKeyboard != null)
                currentKeyboard.text = value;
            lastInput = value;
            onValueChanged?.Invoke(lastInput);
        }
    }

    public UnityEvent<string> onValueChanged { get; private set; }
    public UnityEvent<string> onEditEnd { get; private set; }

    public bool isOpen => currentKeyboard != null;

    private TouchScreenKeyboard currentKeyboard;
    private string lastInput = null;
    private bool canInput = false;

    private void Awake() {
        onValueChanged = new UnityEvent<string>();
        onEditEnd = new UnityEvent<string>();
    }

    private void Update() {
        if (!isOpen)
            return;

        if (!canInput)
            return;

        if (currentKeyboard.text != lastInput) {
            lastInput = currentKeyboard.text;
            onValueChanged.Invoke(lastInput);
        }

        switch (currentKeyboard.status) {
            case TouchScreenKeyboard.Status.Done:
                if (lastInput.Length > 2) {
                    lastInput = lastInput.Substring(0, 2);
                    currentKeyboard.text = lastInput;
                }
                CloseKeyboard();
                onEditEnd.Invoke(lastInput);
                break;
            case TouchScreenKeyboard.Status.Canceled:
                CloseKeyboard();
                break;
        }
    }

    public void OpenKeyboard(string initialText = "") {
        if (isOpen)
            return;
        SetKeyboardAutoClose(false);
        lastInput = initialText;
        currentKeyboard = TouchScreenKeyboard.Open(initialText, TouchScreenKeyboardType.Default,
                                                    false, false, false, false,
                                                    placeHolderText, 4);
        onValueChanged.Invoke(initialText);
        canInput = true;
    }

    public void CloseKeyboard() {
        if (isOpen)
            currentKeyboard.active = false;
        SetKeyboardAutoClose(true);
        currentKeyboard = null;
        canInput = false;
    }

    public void SetInputEnable(bool canInput) {
        if (!isOpen) {
            this.canInput = false;
            return;
        }
        if (canInput)
            ClearKeyboardText();
        this.canInput = canInput;
    }

    private void ClearKeyboardText() {
        if (!isOpen)
            return;
        currentKeyboard.text = string.Empty;
        lastInput = string.Empty;
        onValueChanged.Invoke(lastInput);
    }

#warning Need Keyboard iOS Settings
    private void SetKeyboardAutoClose(bool isAuto) {
#if UNITY_ANDROID
        TouchScreenKeyboard.Android.closeKeyboardOnOutsideTap = isAuto;
#endif
    }
}