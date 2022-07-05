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
    public bool isOpen => currentKeyboard != null;
    public UnityEvent<string> onValueChanged { get; private set; }
    public UnityEvent<string> onEditEnd { get; private set; }

    private TouchScreenKeyboard currentKeyboard;
    private string lastInput = null;
    private bool canInput = true;

    private void Awake() {
        onValueChanged = new UnityEvent<string>();
        onEditEnd = new UnityEvent<string>();
    }

    private void Update() {
        if (!isOpen || !canInput)
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
        TouchScreenKeyboard.Android.closeKeyboardOnOutsideTap = false;
        currentKeyboard = TouchScreenKeyboard.Open(initialText, TouchScreenKeyboardType.Default,
                                                    false, false, false, false,
                                                    placeHolderText, 4);
    }

    public void CloseKeyboard() {
        if (isOpen)
            currentKeyboard.active = false;
        TouchScreenKeyboard.Android.closeKeyboardOnOutsideTap = true;
        currentKeyboard = null;
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