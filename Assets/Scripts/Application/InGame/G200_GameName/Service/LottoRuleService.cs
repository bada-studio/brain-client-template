using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LottoRuleService : Singleton<LottoRuleService>, IService
{
    public ServiceType type => ServiceType.Rule;

    public LottoRule rule { get; private set; }

    public async Task<bool> Initialize(ServiceStatePresenter presenter)
    {
        var text = PersistenceUtil.LoadTextResource("Rules/G200_GameName/G200");
        if (string.IsNullOrEmpty(text))
        {
            // 디폴트 세팅
            rule = new LottoRule
            {
                puzzleMaxCount = 10,
                correctNumberScore = 1000,
                mistakeNumberScore = 400,
                waitToStartTime = 2.0f,
                waitToRememberTime = 2.0f,
                puzzleCountMap = new Dictionary<Difficulty, List<int>>()
            };
            rule.puzzleCountMap[Difficulty.Normal] = new List<int>
            {
                8000, 2000, 0
            };
            rule.puzzleCountMap[Difficulty.Hard] = new List<int>
            {
                6000, 4000, 0
            };
            rule.puzzleCountMap[Difficulty.VeryHard] = new List<int>
            {
                4000, 4000, 2000
            };


            Debug.Log(JsonConvert.SerializeObject(rule));
        }
        else
        {
            rule = JsonConvert.DeserializeObject<LottoRule>(text);
        }

        return true;
    }

    public int CalcScore(int totalAnswerCount, bool isUsedHint, List<int> userMistakeList, List<int> missedAnswerList)
    {
        userMistakeList.Sort();
        missedAnswerList.Sort();

        int score = (totalAnswerCount - userMistakeList.Count) * rule.correctNumberScore;
        if (isUsedHint)
        {
            score = Mathf.FloorToInt(score * rule.hintPanelty);
            score -= (score % 10);
        }

        foreach (var answer in userMistakeList.Zip(missedAnswerList, System.Tuple.Create))
        {
            int mistakeScore = Mathf.FloorToInt(Mathf.Sqrt(((45 - Mathf.Abs(answer.Item1 - answer.Item2)) / 44.0f)) * rule.mistakeNumberScore);
            if (isUsedHint)
                mistakeScore = Mathf.FloorToInt(mistakeScore * rule.hintPanelty);

            score += (mistakeScore - (mistakeScore % 10));
        }

        return score;
    }

    public int CalcGrade(int correctCount)
    {
        switch (correctCount)
        {
            case 6:
                return 1;

            case 5:
                return (Random.Range(0, 10000) < 1000) ? 2 : 3;     // NOTE : 100%를 int로 10000

            case 4:
                return 4;

            case 3:
                return 5;

            default:
                return 99;
        }
    }

    public int GetAnswerCount(Difficulty difficulty)
    {
        if (rule.puzzleCountMap.TryGetValue(difficulty, out List<int> countPercentList))
        {
            int randomResult = Random.Range(0, 10000);
            for (int i = 0; i < countPercentList.Count; ++i)
            {
                if (randomResult < countPercentList[i])
                    return 3 + i;

                randomResult -= countPercentList[i];
            }
        }

        return 3;
    }
}
