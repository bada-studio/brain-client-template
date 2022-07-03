using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    public class AnswerCell : MonoBehaviour {
        [SerializeField] private Color normalColor;
        [SerializeField] private Color answerColor;
        [SerializeField] private Color wrongImgColor;
        [SerializeField] private Color wrongTextColor;

        [SerializeField] private Color alertTextColor;

        [SerializeField] private Text answerText;
        [SerializeField] private Image bgImage;

        public void UpdateCharacter(char ch) {
            bgImage.color = normalColor;
            answerText.text = ch.ToString();
        }

        public void ShowCorrectUI() {
            bgImage.color = answerColor;
            answerText.gameObject.SetActive(true);
            answerText.color = Color.black;
        }

        public void ShowIncorrectUI() {
            bgImage.color = wrongImgColor;
            answerText.color = wrongTextColor;
        }

        public void ChangeTextColor(bool isAlert) {
            answerText.color = isAlert ? alertTextColor : Color.black;
        }
    }
}