using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    public class AnswerCell : MonoBehaviour {
        [SerializeField] private Text answerText;
        [SerializeField] private Image bgImage;

        public void SetShow(char ch) {
            answerText.text = ch.ToString();
        }

        public void SetHide(char ch) {
            answerText.text = ch.ToString();
            answerText.gameObject.SetActive(false);
        }

        public void ShowHideAnswer() {
            answerText.gameObject.SetActive(true);
        }
    }
}