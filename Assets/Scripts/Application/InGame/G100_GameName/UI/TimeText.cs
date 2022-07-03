using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    public class TimeText : MonoBehaviour, IUIUpdateCallback, IUIEventCallback {
        [SerializeField] private Text timeText;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color alertTextColor;
        private int lastRemainTime;

        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameData gameData, BCPG9PlayData playData, string input = null) {
            switch (eventType) {
                case BCPG9GameEventType.Reset:
                    timeText.color = normalColor;
                    break;
                case BCPG9GameEventType.CloseEnd:
                    timeText.color = alertTextColor;
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