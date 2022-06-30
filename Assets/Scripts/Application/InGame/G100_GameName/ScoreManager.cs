using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCPG9 {
    public class ScoreManager : MonoBehaviour {
        [SerializeField] private Timer timer;
        [SerializeField] private int standardScore;
        [SerializeField] private float comboCheckTime;
        [SerializeField] private int maxComboCount;
        [SerializeField] private float comboMultiplier;
        [SerializeField] private float passMultiplier;

        public float comboTime => comboCheckTime;
        public int comboCount { get; private set; }
        private float lastCheckTime;
        private int currentScore;
        private string currentAnswer;

        public void ResetScore() {
            lastCheckTime = timer.time;
            comboCount = 0;
            currentScore = 0;
        }

        public void SetAnswer(BCPG9Rule rule) {
            currentAnswer = rule.word.Substring(2, 2);
        }

        public bool CheckAnswer(string answer) {
            bool isCorrect = currentAnswer.CompareTo(answer) == 0;
            CheckCombo(isCorrect);
            if (isCorrect)
                AddScore();
            return isCorrect;
        }

        public void PassAnswer() {
            currentScore = Mathf.FloorToInt(currentScore * passMultiplier);
        }

        private void Start() {
            ResetScore();
        }

        private void CheckCombo(bool isCorrect) {
            var isCombo = (timer.time - lastCheckTime < comboCheckTime || comboCount <= 0) && isCorrect;
            if (isCombo)
                comboCount++;
            else
                comboCount = 0;
            lastCheckTime = timer.time;
        }

        private void AddScore() {
            var multiplier = Mathf.Pow(comboMultiplier, comboCount);
            currentScore += Mathf.FloorToInt(standardScore * multiplier);
        }
    }
}