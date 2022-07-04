using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LottoRule
{
    public int puzzleMaxCount;
    public float waitToStartTime;
    public float waitToRememberTime;
    public int correctNumberScore;
    public int mistakeNumberScore;
    public float showHintTime;
    public float hintPanelty;
    public Dictionary<Difficulty, List<int>> puzzleCountMap;
}