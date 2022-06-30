using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace BCPG9 {
    public class InGameUIController : MonoBehaviour {
        [SerializeField] InputField answerInputField;

        private List<IUICallbacks> callbacks;
        private BCPG9_FourWord gameManager;
        private Action<string> answerAction;

        public async Task<bool> Initilize(BCPG9_FourWord gameManager, Action<string> answerAction) {
            callbacks = GetComponentsInChildren<IUICallbacks>().ToList();
            this.answerAction = answerAction;
            answerInputField.onValueChanged.AddListener(OnInputValueChange);
            return true;
        }

        /*  todo
            키보드 상에서 한글 입력 시 버퍼로 인해 제대로 입력 안됨
            모바일 상에서는 정상입력
        */
        public void OnInputValueChange(string value) {
            answerAction?.Invoke(value);
        }

        public void PauseUI() => callbacks.ForEach(_ => _.OnPause());
        public void ResumeUI() => callbacks.ForEach(_ => _.OnResume());
        public void ResetUI(float comboCheckTime)
            => callbacks.ForEach(_ => _.OnResetGame(comboCheckTime));
        public void UpdateAnswerUI(BCPG9Rule answer, float gameTime)
            => callbacks.ForEach(_ => _.OnUpdateAnswer(answer, gameTime));
        public void ResultAnswerUI(bool isCorrect, int score, float gameTime, int comboCount)
            => callbacks.ForEach(_ => _.OnAnswer(isCorrect, score, gameTime, comboCount));

    }
}