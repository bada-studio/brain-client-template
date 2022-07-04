using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    /*
        Header Progress Bar Slider Component Event Handler
    */
    public class ProgressSlider : MonoBehaviour, IUIUpdateCallback, IUIEventCallback {
        [SerializeField] Slider progressSlider;
        [SerializeField] Image fillImage;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color alertColor;

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameSettings gameData, BCPG9PlayData playData, string input = null) {
            switch (eventType) {
                case BCPG9GameEventType.CloseEnd:
                    fillImage.color = alertColor;
                    break;
                case BCPG9GameEventType.Reset:
                    fillImage.color = normalColor;
                    break;
            }
        }

        public void OnUpdateCall(BCPG9PlayData playData) {
            var ratio = playData.remainTimeRatio > 0 ? playData.remainTimeRatio : 0;
            progressSlider.value = ratio;
        }
    }
}