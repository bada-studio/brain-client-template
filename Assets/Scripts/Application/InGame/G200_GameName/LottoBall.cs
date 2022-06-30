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

    public Text ballNumberText;
    public List<Color> lottoColors;
    public Color cursorColor;
    public Color emptyColor;

    private Image ballImage;

    private void Awake()
    {
        ballImage = GetComponent<Image>();
    }

    public void SetNumber(Type ballType, string number)
    {
        gameObject.SetActive(true);

        int numberInt = int.Parse(number);
        switch (ballType)
        {
            case Type.Normal:
                ballNumberText.text = number;

                if (0 == numberInt || 45 < numberInt)
                    ballImage.color = lottoColors[lottoColors.Count - 1];
                else
                    ballImage.color = lottoColors[(numberInt - 1) / 10];
                break;

            case Type.Cursor:
                ballNumberText.text = "";
                ballImage.color = cursorColor;
                break;

            case Type.Empty:
                ballNumberText.text = "";
                ballImage.color = emptyColor;
                break;
        }

        if (0.5f < ballImage.color.r || 0.5f < ballImage.color.g || 0.5f < ballImage.color.b)
            ballNumberText.color = Color.black;
        else
            ballNumberText.color = Color.white;
    }

    public void SetNumber(Type ballType, int number) => SetNumber(ballType, number.ToString());

    public int GetNumber() => int.Parse(ballNumberText.text);
}
