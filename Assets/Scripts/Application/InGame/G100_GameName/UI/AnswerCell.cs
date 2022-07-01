using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    public class AnswerCell : MonoBehaviour {
        [SerializeField] private Color normalColor;
        [SerializeField] private Color answerColor;

        [SerializeField] private Text answerText;
        [SerializeField] private Image bgImage;

        public void SetShow(char ch) {
            bgImage.color = normalColor;
            answerText.text = ch.ToString();
        }

        public void SetHide(char ch) {
            bgImage.color = normalColor;
            answerText.text = ch.ToString();
            answerText.gameObject.SetActive(false);
        }

        public void ShowHideAnswer() {
            bgImage.color = answerColor;
            answerText.gameObject.SetActive(true);
        }
    }
}