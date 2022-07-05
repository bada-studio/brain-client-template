using System;
using UnityEngine;
using UnityEngine.Events;

public class CustomInputField : MonoBehaviour {
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
    public bool isOpen { get; private set; }
    public UnityEvent<string> onValueChanged { get; private set; }

    private TouchScreenKeyboard currentKeyboard;
    private string lastInput = null;
    private bool canInput = true;

    private void Awake() {
        onValueChanged = new UnityEvent<string>();
    }

    private void Update() {
        if (!isOpen || !canInput)
            return;
        if (currentKeyboard.text == lastInput)
            return;
        lastInput = currentKeyboard.text;
        onValueChanged.Invoke(lastInput);
    }

    public void OpenKeyboard() {
        if (isOpen)
            return;
        TouchScreenKeyboard.Android.closeKeyboardOnOutsideTap = false;
        currentKeyboard = TouchScreenKeyboard.Open(null, TouchScreenKeyboardType.Default,
                                                    false, false, false, false, null, 2);
        isOpen = true;
    }

    public void CloseKeyboard() {
        if (!isOpen)
            return;
        TouchScreenKeyboard.Android.closeKeyboardOnOutsideTap = true;
        currentKeyboard.active = false;
        isOpen = false;
    }

    public void SetInputEnable(bool canInput) {
        if (!isOpen)
            return;

        if (canInput) {
            this.canInput = true;
            currentKeyboard.text = string.Empty;
            lastInput = string.Empty;
            onValueChanged.Invoke(lastInput);
        } else {
            this.canInput = false;
        }
    }
}