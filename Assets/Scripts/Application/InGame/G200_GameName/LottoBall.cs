using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LottoBall : MonoBehaviour
{
    public enum Type
    {
        Normal,
        Cursor,
        Empty
    }

    public delegate void OnChangedFocusLottoBall(LottoBall activeLottoBall);

    public Text ballNumberText;
    public List<Color> lottoColors;
    public Color cursorColor;
    public Color emptyColor;

    private Image ballImage;
    private Button ballButton;
    private OnChangedFocusLottoBall callback;

    private void Awake()
    {
        ballImage = GetComponent<Image>();
        ballButton = GetComponent<Button>();
    }

    public void SetNumber(Type ballType, string number, bool isInteractable = false, OnChangedFocusLottoBall callback = null)
    {
        gameObject.SetActive(true);

        int numberInt = string.IsNullOrEmpty(number) ? 0 : int.Parse(number);
        if (numberInt == 0 && ballType == Type.Normal)
            ballType = Type.Empty;

        switch (ballType)
        {
            case Type.Normal:
                ballNumberText.text = number;

                if (numberInt <= 45)
                    ballImage.color = lottoColors[(numberInt - 1) / 10];
                else
                    ballImage.color = lottoColors[lottoColors.Count - 1];
                break;

            case Type.Cursor:
                ballNumberText.text = number;

                if (string.IsNullOrEmpty(ballNumberText.text))
                    ballImage.color = cursorColor;
                else if (numberInt <= 45)
                    ballImage.color = Color.Lerp(lottoColors[(numberInt - 1) / 10], Color.white, 0.5f);
                else
                    ballImage.color = Color.Lerp(lottoColors[lottoColors.Count - 1], Color.white, 0.5f);
                break;

            case Type.Empty:
                ballNumberText.text = "";
                ballImage.color = emptyColor;
                break;
        }

        ballButton.interactable = isInteractable;
        this.callback = callback;

        if (0.5f < ballImage.color.r || 0.5f < ballImage.color.g || 0.5f < ballImage.color.b)
            ballNumberText.color = Color.black;
        else
            ballNumberText.color = Color.white;
    }

    public void SetNumber(Type ballType, int number = -1, bool isInteractable = false, OnChangedFocusLottoBall callback = null) => SetNumber(ballType, (number == -1) ? ballNumberText.text : number.ToString(), isInteractable, callback);

    public int GetNumber() => string.IsNullOrEmpty(ballNumberText.text) ? 0 : int.Parse(ballNumberText.text);

    public string GetNumberString() => ballNumberText.text;

    public void OnClickLottoBall()
    {
        callback?.Invoke(this);
    }
}
