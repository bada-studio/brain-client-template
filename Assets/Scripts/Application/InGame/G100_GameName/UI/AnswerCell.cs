using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    public class AnswerCell : MonoBehaviour {
        [SerializeField] private Text answerText;
        [SerializeField] private Image bgImage;

        public void UpdateColor(Color imageColor, Color textColor) {
            bgImage.color = imageColor;
            answerText.color = textColor; 
        }

        public void UpdateCharacter(char ch) {
            answerText.text = ch.ToString();
        }
    }
}