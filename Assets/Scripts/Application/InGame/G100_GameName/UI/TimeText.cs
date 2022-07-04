using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    /*
        Header Time Text Component Event Handler
    */
    public class TimeText : MonoBehaviour, IUIUpdateCallback, IUIEventCallback {
        [SerializeField] private Text timeText;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color alertTextColor;
        [SerializeField] private Animation textAnim;
        private int lastRemainTime;

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameSettings gameData, BCPG9PlayData playData, string input = null) {
            switch (eventType) {
                case BCPG9GameEventType.Reset:
                    timeText.color = normalColor;
                    textAnim.Rewind();
                    textAnim.Stop();
                    break;
                case BCPG9GameEventType.CloseEnd:
                    timeText.color = alertTextColor;
                    textAnim.Play();
                    break;
                case BCPG9GameEventType.End:
                    textAnim.Rewind();
                    textAnim.Stop();
                    break;
            }
        }

        public void OnUpdateCall(BCPG9PlayData playData) {
            if (lastRemainTime != playData.remainTime) {
                lastRemainTime = Mathf.CeilToInt(playData.remainTime);
                timeText.text = $"{lastRemainTime.ToString()}초";
            }
        }
    }
}