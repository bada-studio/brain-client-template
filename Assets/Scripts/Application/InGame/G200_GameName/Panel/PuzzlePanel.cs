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

    private bool isMovedCursor;

    private bool isUsedHint;

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

        isMovedCursor = false;
        isUsedHint = false;
        cursorBallIndex = 0;
        cursorNumber = "";
        numberList[cursorBallIndex].SetNumber(LottoBall.Type.Cursor, "", true, OnClickLottoBall);

        for (int i = 1; i < gameController.AnswerCount; ++i)
        {
            numberList[i].SetNumber(LottoBall.Type.Empty, 0, true, OnClickLottoBall);
        }

        for (int i = 0; i < gameController.AnswerCount; ++i)
        {
            hintNumberList[i].SetNumber(LottoBall.Type.Normal, gameController.AnswerList[i]);
        }
    }

    public void OnClickLottoBall(LottoBall activeLottoBall)
    {
        if (cursorBallIndex < gameController.AnswerCount)
        {
            LottoBall prevCursorBall = numberList[cursorBallIndex];
            if (activeLottoBall == prevCursorBall)
                return;

            prevCursorBall.SetNumber(LottoBall.Type.Normal, -1, true, OnClickLottoBall);
        }

        for (int i = 0; i < numberList.Count; ++i)
        {
            if (activeLottoBall == numberList[i])
            {
                cursorBallIndex = i;
                cursorNumber = activeLottoBall.GetNumberString();
                break;
            }
        }

        numberList[cursorBallIndex].SetNumber(LottoBall.Type.Cursor, -1, true, OnClickLottoBall);

        isMovedCursor = true;
    }

    public void OnClickKeypadNumber(string number)
    {
        if (gameController.AnswerCount <= cursorBallIndex)
            return;

        if (isMovedCursor)
        {
            isMovedCursor = false;
            cursorNumber = "";
        }

        cursorNumber += number;
        numberList[cursorBallIndex].SetNumber(LottoBall.Type.Cursor, cursorNumber, true, OnClickLottoBall);

        if (2 <= cursorNumber.Length)
        {
            if (ValidateInputLottoNumber(numberList[cursorBallIndex]))
            {
                numberList[cursorBallIndex].SetNumber(LottoBall.Type.Normal, numberList[cursorBallIndex].GetNumber(), true, OnClickLottoBall);

                ++cursorBallIndex;
                cursorNumber = "";
                if (cursorBallIndex < gameController.AnswerCount)
                {
                    numberList[cursorBallIndex].SetNumber(LottoBall.Type.Cursor, -1, true, OnClickLottoBall);
                    cursorNumber = numberList[cursorBallIndex].GetNumberString();
                }
            }
            else
            {
                cursorNumber = "";
                numberList[cursorBallIndex].SetNumber(LottoBall.Type.Cursor, cursorNumber, true, OnClickLottoBall);
            }
        }

        confirmButton.interactable = ValidateLottoNumbers();
    }

    public void OnClickKeypadBack()
    {
        if (string.IsNullOrEmpty(cursorNumber) && 0 < cursorBallIndex)
        {
            if (cursorBallIndex != gameController.AnswerCount)
                numberList[cursorBallIndex].SetNumber(LottoBall.Type.Empty, 0, true, OnClickLottoBall);

            --cursorBallIndex;
        }

        cursorNumber = numberList[cursorBallIndex].GetNumberString();
        if (!string.IsNullOrEmpty(cursorNumber))
            cursorNumber = cursorNumber.Substring(0, cursorNumber.Length - 1);

        numberList[cursorBallIndex].SetNumber(LottoBall.Type.Cursor, cursorNumber, true, OnClickLottoBall);

        confirmButton.interactable = ValidateLottoNumbers();
    }

    public void OnClickConfirm()
    {
        gameController.StartCoroutine(CoResultPuzzle());
    }

    public void OnClickHint()
    {
        isUsedHint = true;
        gameController.StartCoroutine(CoEnableHint());
    }

    private IEnumerator CoEnableHint()
    {
        hintNumberBallRoot.gameObject.SetActive(true);

        yield return new WaitForSeconds(Service.lottoRule.rule.showHintTime);

        hintNumberBallRoot.gameObject.SetActive(false);
    }

    private bool ValidateInputLottoNumber(LottoBall numberBall)
    {
        int number = numberBall.GetNumber();
        if (45 < number)
            return false;

        foreach (var element in numberList)
        {
            if (!element.gameObject.activeSelf)
                continue;

            if (numberBall == element)
                continue;

            if (element.GetNumber() == number)
                return false;
        }

        return true;
    }

    private bool ValidateLottoNumbers()
    {
        foreach (var leftElement in numberList)
        {
            if (!leftElement.gameObject.activeSelf)
                continue;

            var leftNumber = leftElement.GetNumber();
            if (leftNumber == 0 || 45 < leftNumber)
                return false;

            foreach (var rightElement in numberList)
            {
                if (!rightElement.gameObject.activeSelf)
                    continue;

                if (leftElement == rightElement)
                    continue;

                if (leftNumber == rightElement.GetNumber())
                    return false;
            }
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
            int score = Service.lottoRule.CalcScore(gameController.AnswerCount, isUsedHint, userMistakeList, missedList);
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
