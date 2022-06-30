/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author Ha Sungmin <sm.ha@biscuitlabs.io>
* @created 2022/06/27
* @desc G100_GameName Template
*/
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnControls;
using DG.Tweening;

public enum Difficulty
{
    Normal,
    Hard,
    VeryHard
}

namespace g200
{
    /*기능설명*/
    public class G200_GameName : MonoBehaviour
    {
        public Animator gameAnimator;

        public StartPanel startPanel;
        public DreamPanel dreamPanel;
        public PuzzlePanel puzzlePanel;

        public Text totalScoreText;
        public Text playTimeText;
        public Slider puzzleCountSlider;

        [Header("MetaData")]
        public Difficulty difficulty;
        public int stage;

        private List<int> answerList = new List<int>();
        private List<int> restAnswerList = new List<int>();
        private int totalScore;
        private int puzzleCount;
        private float elapsedPlayTime;
        private bool isAccumulatePlayTime;

        public int AnswerCount => answerList.Count;
        public List<int> AnswerList => answerList;
        public List<int> RestAnswerList => restAnswerList;

        private void Awake()
        {
            startPanel.SetController(this);
            dreamPanel.SetController(this);
            puzzlePanel.SetController(this);

            startPanel.Initialize();

            Random.InitState(System.DateTime.Now.Millisecond);
        }

        private void Update()
        {
            if (isAccumulatePlayTime)
            {
                elapsedPlayTime += Time.unscaledDeltaTime;

                playTimeText.text = string.Format("{0:D}초", Mathf.FloorToInt(elapsedPlayTime));
            }
        }

        public void OnClickStartGame()
        {
            totalScore = 0;
            puzzleCount = 0;
            AddTotalScore(totalScore);
            elapsedPlayTime = 0.0f;

            ReplayPuzzle();
        }

        public void OnClickCloseGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }

        public void ReplayPuzzle()
        {
            if (puzzleCount < Service.lottoRule.rule.puzzleMaxCount)
            {
                gameAnimator.Play("StartGame");
            }
            else
            {
                Service.userData.BestScore = totalScore;
                Service.userData.SaveUserData();

                // TODO : 결과 화면 도출
                Debug.Log("끝 결과 화면 도출");
                Debug.Log(string.Format("로또번호 기억하기 {0}", Utility.GetDifficultyText(difficulty)));
                Debug.Log(string.Format("최고 점수 {0:N0}", Service.userData.BestScore));
                Debug.Log(string.Format("점수 {0:N0}", totalScore));
                Debug.Log(string.Format("플레이 시간 {0:D}초", Mathf.FloorToInt(elapsedPlayTime)));
                Debug.Log(string.Format("운동성과 상위{0}%", 20));
            }
        }

        public void InitializeGame()
        {
            startPanel.gameObject.SetActive(false);

            dreamPanel.Initialize();
            puzzlePanel.Initialize();

            ++puzzleCount;
            isAccumulatePlayTime = false;
            puzzleCountSlider.value = (Service.lottoRule.rule.puzzleMaxCount != 0) ? puzzleCount / (float)Service.lottoRule.rule.puzzleMaxCount : 1.0f;
        }

        public void StartGame()
        {
            MakeAnswer(Service.lottoRule.GetAnswerCount(difficulty));

            StartCoroutine(CoStartGame());
        }

        public void ToQuestionPanel()
        {
            dreamPanel.gameObject.SetActive(false);
            puzzlePanel.SetAnswer();
        }

        public void AddTotalScore(int newScore)
        {
            totalScore += newScore;
            totalScoreText.text = totalScore.ToString();
        }

        public void StopAccumulatePlayTime()
        {
            isAccumulatePlayTime = false;
        }

        private void MakeAnswer(int count)
        {
            answerList.Clear();
            restAnswerList.Clear();

            List<int> numberBox = new List<int>();
            for (int i = 1; i <= 45; ++i)
            {
                numberBox.Add(i);
            }

            for (int i = 0; i < count; ++i)
            {
                int randomIndex = Random.Range(0, numberBox.Count);

                answerList.Add(numberBox[randomIndex]);
                numberBox.RemoveAt(randomIndex);
            }

            for (int i = 0; i < 6 - count; ++i)
            {
                int randomIndex = Random.Range(0, numberBox.Count);

                restAnswerList.Add(numberBox[randomIndex]);
                numberBox.RemoveAt(randomIndex);
            }
        }

        private IEnumerator CoStartGame()
        {
            dreamPanel.StartGame();

            yield return new WaitForSeconds(Service.lottoRule.rule.waitToStartTime);

            dreamPanel.ShowNumbers();

            yield return new WaitForSeconds(Service.lottoRule.rule.waitToRememberTime);

            isAccumulatePlayTime = true;
            gameAnimator.Play("ToQuestionPanel");
        }
    }
}
