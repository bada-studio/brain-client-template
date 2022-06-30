using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class PuzzlePanel : BasePanel
{
    public CanvasGroup keypadCanvasGroup;
    public GameObject resultLabelBg;
    public Text resltLabelText;
    public Text description;

    public List<LottoBall> numberList;
    public List<SkeletonGraphic> resultMarkList;

    public GameObject hintNumberBallRoot;
    public List<LottoBall> hintNumberList;

    public Button confirmButton;

    [SerializeField]
    private Color correctColor;
    [SerializeField]
    private Color incorrectColor;

    private Animator puzzleAnimator;

    private int cursorBallIndex;
    private string cursorNumber;

    private void Awake()
    {
        puzzleAnimator = GetComponent<Animator>();
        hintNumberBallRoot.gameObject.SetActive(true);
    }

    public void Initialize()
    {
        gameObject.SetActive(false);

        foreach (var numberBall in numberList)
        {
            numberBall.gameObject.SetActive(false);
        }

        foreach (var resultMark in resultMarkList)
        {
            resultMark.gameObject.SetActive(false);
        }

        hintNumberBallRoot.gameObject.SetActive(false);
        foreach (var numberBall in hintNumberList)
        {
            numberBall.gameObject.SetActive(false);
        }

        resultLabelBg.SetActive(false);
    }

    public void SetAnswer()
    {
        gameObject.SetActive(true);

        puzzleAnimator.Play("PuzzleReset");
        keypadCanvasGroup.gameObject.SetActive(true);
        keypadCanvasGroup.blocksRaycasts = true;

        confirmButton.interactable = false;

        description.text = "꿈에서 본 숫자를 입력하세요.";

        cursorBallIndex = 0;
        cursorNumber = "";
        numberList[cursorBallIndex].SetNumber(LottoBall.Type.Cursor, 0);

        for (int i = 1; i < gameController.AnswerCount; ++i)
        {
            numberList[i].SetNumber(LottoBall.Type.Empty, 0);
        }

        for (int i = 0; i < gameController.AnswerCount; ++i)
        {
            hintNumberList[i].SetNumber(LottoBall.Type.Normal, gameController.AnswerList[i]);
        }
    }

    public void OnClickKeypadNumber(string number)
    {
        if (gameController.AnswerCount <= cursorBallIndex)
            return;

        cursorNumber += number;
        numberList[cursorBallIndex].SetNumber(LottoBall.Type.Normal, cursorNumber);

        if (2 <= cursorNumber.Length)
        {
            if (ValidateInputLottoNumber())
            {
                ++cursorBallIndex;
                cursorNumber = "";
                if (gameController.AnswerCount <= cursorBallIndex)
                    confirmButton.interactable = true;
                else
                    numberList[cursorBallIndex].SetNumber(LottoBall.Type.Cursor, 0);
            }
            else
            {
                cursorNumber = "";
                numberList[cursorBallIndex].SetNumber(LottoBall.Type.Cursor, 0);
            }
        }
    }

    public void OnClickKeypadBack()
    {
        if (string.IsNullOrEmpty(cursorNumber) && 0 < cursorBallIndex)
        {
            if (cursorBallIndex != gameController.AnswerCount)
                numberList[cursorBallIndex].SetNumber(LottoBall.Type.Empty, 0);

            --cursorBallIndex;
            confirmButton.interactable = false;
        }

        numberList[cursorBallIndex].SetNumber(LottoBall.Type.Cursor, 0);
        cursorNumber = "";
    }

    public void OnClickConfirm()
    {
        gameController.StartCoroutine(CoResultPuzzle());
    }

    public void OnClickHint()
    {
        gameController.StartCoroutine(CoEnableHint());
    }

    private IEnumerator CoEnableHint()
    {
        hintNumberBallRoot.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        hintNumberBallRoot.gameObject.SetActive(false);
    }

    private bool ValidateInputLottoNumber()
    {
        int number = numberList[cursorBallIndex].GetNumber();
        if (45 < number)
            return false;

        for (int i = 0; i < cursorBallIndex; ++i)
        {
            if (numberList[i].GetNumber() == number)
                return false;
        }

        return true;
    }

    private IEnumerator CoResultPuzzle()
    {
        {
            description.text = "결과는...!";

            keypadCanvasGroup.blocksRaycasts = false;
            puzzleAnimator.Play("PuzzleConfirm");

            gameController.StopAccumulatePlayTime();

            for (int i = gameController.AnswerCount; i < 6; ++i)
            {
                int restAnswer = gameController.RestAnswerList[i - gameController.AnswerCount];
                numberList[i].SetNumber(LottoBall.Type.Normal, restAnswer);
            }
        }

        yield return new WaitForSeconds(0.2f);

        List<int> userMistakeList = new List<int>();
        List<int> missedList = new List<int>();
        {
            List<int> userCorrectList = new List<int>();
            for (int i = 0; i < gameController.AnswerCount; ++i)
            {
                int answer = numberList[i].GetNumber();
                var foundAnswer = gameController.AnswerList.Find((x) => { return x == answer; });

                bool isCorrect = (foundAnswer == answer);
                if (isCorrect)
                {
                    userCorrectList.Add(answer);
                }
                else
                {
                    userMistakeList.Add(answer);
                }

                StartCoroutine(CoEnableResultMark(i, isCorrect, i * 0.05f));
            }

            foreach (var answer in gameController.AnswerList)
            {
                var foundAnswer = userCorrectList.Find((x) => { return x == answer; });

                if (foundAnswer != answer)
                    missedList.Add(answer);
            }

            for (int i = gameController.AnswerCount; i < 6; ++i)
            {
                StartCoroutine(CoEnableResultMark(i, true, i * 0.05f));
            }
        }

        yield return new WaitForSeconds(0.3f);

        {
            int score = Service.lottoRule.CalcScore(gameController.AnswerCount, userMistakeList, missedList);
            gameController.AddTotalScore(score);

            int grade = Service.lottoRule.CalcGrade(6 - userMistakeList.Count);
            resltLabelText.text = (grade <= 5) ? string.Format("{0}등 당첨!", grade) : "다음 기회에!";

            resultLabelBg.gameObject.SetActive(true);
            resultLabelBg.transform.localScale = Vector3.zero;
            yield return StartCoroutine(resultLabelBg.ScaleTo(EaseType.easeOutElastic, 2.0f, Vector3.one));
        }

        yield return new WaitForSeconds(1.0f);

        gameController.ReplayPuzzle();
    }

    private IEnumerator CoEnableResultMark(int index, bool isCorrect, float waitingTime)
    {
        if (0.0f < waitingTime)
            yield return new WaitForSeconds(waitingTime);

        var resultMark = resultMarkList[index];
        resultMark.gameObject.SetActive(true);
        if (isCorrect)
        {
            resultMark.Skeleton.SetSkin("skin1");
            resultMark.color = correctColor;
        }
        else
        {
            resultMark.Skeleton.SetSkin("skin2");
            resultMark.color = incorrectColor;
        }

        resultMark.AnimationState.SetAnimation(0, "start", false);
        yield return new WaitForSeconds(0.3f);
        resultMark.freeze = true;
    }
}
