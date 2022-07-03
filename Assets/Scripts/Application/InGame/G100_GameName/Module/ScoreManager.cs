using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCPG9 {
    public class ScoreManager : MonoBehaviour, IGameModule {
        [SerializeField] private Timer timer;

        public int comboCount { get; private set; }
        public int currentScore { get; private set; }
        public Dictionary<string, string> currentAnswer { get; private set; }
        public float remainComboTime => comboCheckTime - (timer.time - lastCheckTime);

        private float lastCheckTime;
        private int standardScore;
        private float comboCheckTime;
        private int maxComboCount;
        private float comboMultiplier;
        private float passMultiplier;

        public void Initialize(BCPG9GameSettings gameData, BCPG9_FourWord gameManager) {
            this.standardScore = gameData.standardScore;
            this.comboCheckTime = gameData.comboCheckTime;
            this.maxComboCount = gameData.maxComboCount;
            this.comboMultiplier = gameData.comboMultiplier;
            this.passMultiplier = gameData.passMultiplier;
        }

        public void ResetModule() {
            lastCheckTime = timer.time;
            comboCount = 0;
            currentScore = 0;
        }

        public void SetAnswer(BCPG9Rule rule) {
            currentAnswer = rule.answer;
        }

        public bool CheckAnswer(string input) {
            bool isCorrect = currentAnswer.ContainsKey(input);
            if (isCorrect) {
                CheckCombo();
                AddScore();
            }
            return isCorrect;
        }

        public void PassAnswer() {
            currentScore = Mathf.FloorToInt(currentScore * passMultiplier);
        }

        public void ClearCombo() {
            comboCount = 0;
        }

        public void CheckCombo() {
            var isCombo = remainComboTime > 0;
            Debug.Log($"remain combo time {remainComboTime}");
            if (isCombo)
                comboCount++;
            else
                comboCount = 0;
            lastCheckTime = timer.time;
        }

        private void AddScore() {
            var multiplier = Mathf.Pow(comboMultiplier, comboCount - 1);
            multiplier = multiplier < 1 ? 1 : multiplier;
            currentScore += Mathf.FloorToInt(standardScore * multiplier);
            Debug.Log($"{multiplier} {currentScore}");
        }
    }
}