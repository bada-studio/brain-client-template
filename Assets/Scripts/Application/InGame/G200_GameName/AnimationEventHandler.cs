using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public g200.G200_GameName gameController;

    public void OnEndedAnimation(AnimatorStateInfo stateInfo)
    {
        if (stateInfo.IsName("StartGame"))
        {
            gameController.StartGame();
        }
    }

    public void OnCloseStartPage()
    {
        gameController.InitializeGame();
    }

    public void OnToQuestionPanel()
    {
        gameController.ToQuestionPanel();
    }
}
