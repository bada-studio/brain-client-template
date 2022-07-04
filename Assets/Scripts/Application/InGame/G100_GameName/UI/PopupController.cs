using UnityEngine;

namespace BCPG9 {
    /*
        Default UI Popup Layer Components Controller
    */
    public class PopupController : MonoBehaviour {
        [SerializeField] private ResultPopup resultPopup;
        [SerializeField] private BottomPanel bottomPanel;
        [SerializeField] private ComboCounter comboCounter;

        public void ShowBottomPanel() {
            bottomPanel.Show();
        }

        public void ShowResult(bool isCorrect, int comboCount) {
            if (isCorrect) {
                resultPopup.OnCorrect();
                comboCounter.ShowCombo(comboCount);
            } else {
                resultPopup.OnWrong();
            }
        }
    }
}