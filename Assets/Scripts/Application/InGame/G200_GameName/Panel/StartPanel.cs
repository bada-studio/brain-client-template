using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    public Text bestScoreText;
    public Text stageText;
    public Text difficultyText;

    public void Initialize()
    {
        gameObject.SetActive(true);

        bestScoreText.text = string.Format("{0:N0}", Service.userData.BestScore);
        stageText.text = string.Format("스테이지 {0:D}", gameController.stage);
        difficultyText.text = string.Format("난이도 / {0}", g200.Utility.GetDifficultyText(gameController.difficulty));
    }
}
