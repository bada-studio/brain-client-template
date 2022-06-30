using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    public class TimeText : MonoBehaviour, IUIUpdateCallback {
        [SerializeField] private Text timeText;
        private int lastRemainTime;

        public void OnUpdateCall(BCPG9PlayData playData) {
            if (lastRemainTime != playData.limitedTime) {
                lastRemainTime = playData.limitedTime;
                timeText.text = $"{lastRemainTime.ToString()}초";
            }
        }
    }
}