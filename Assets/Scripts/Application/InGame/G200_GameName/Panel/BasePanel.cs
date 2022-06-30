using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected g200.G200_GameName gameController;

    public void SetController(g200.G200_GameName gameController)
    {
        this.gameController = gameController;
    }
}
