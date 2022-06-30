using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DreamPanel : BasePanel
{
    public Image pigNose;
    public Text description;
    public List<LottoBall> lottoBallList;

    public void Initialize()
    {
        gameObject.SetActive(true);

        description.text = "";

        foreach (var lottoBall in lottoBallList)
            lottoBall.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        gameController.StartCoroutine(CoAnimatePigNose());

        description.text = "번호가 곧 나옵니다";
    }

    public void ShowNumbers()
    {
        description.text = "번호를 기억하세요!";

        List<int> index = new List<int>();
        for (int i = 0; i < lottoBallList.Count; ++i)
        {
            index.Add(i);
        }

        foreach (var answer in gameController.AnswerList)
        {
            int randomIndex = Random.Range(0, index.Count);

            lottoBallList[index[randomIndex]].SetNumber(LottoBall.Type.Normal, answer);
            index.RemoveAt(randomIndex);
        }
    }

    private IEnumerator CoAnimatePigNose()
    {
        float halfTime = Service.lottoRule.rule.waitToStartTime / 6;

        yield return StartCoroutine(pigNose.ScaleTo(EaseType.linear, halfTime, new Vector3(1.0f, 0.3f, 1.0f)));
        yield return StartCoroutine(pigNose.ScaleTo(EaseType.linear, halfTime, new Vector3(1.0f, 1.0f, 1.0f)));

        yield return StartCoroutine(pigNose.ScaleTo(EaseType.linear, halfTime, new Vector3(1.0f, 0.3f, 1.0f)));
        yield return StartCoroutine(pigNose.ScaleTo(EaseType.linear, halfTime, new Vector3(1.0f, 1.0f, 1.0f)));

        yield return StartCoroutine(pigNose.ScaleTo(EaseType.linear, halfTime, new Vector3(1.0f, 0.3f, 1.0f)));
        yield return StartCoroutine(pigNose.ScaleTo(EaseType.linear, halfTime, new Vector3(1.0f, 1.0f, 1.0f)));
    }
}
