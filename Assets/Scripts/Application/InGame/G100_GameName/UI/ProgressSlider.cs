using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    public class ProgressSlider : MonoBehaviour, IUIUpdateCallback {
        [SerializeField] Slider progressSlider;

        public void OnUpdateCall(BCPG9PlayData playData) {
            var ratio = playData.remainComboRatio > 0 ? playData.remainComboRatio : 0;
            progressSlider.value = ratio;
        }
    }
}