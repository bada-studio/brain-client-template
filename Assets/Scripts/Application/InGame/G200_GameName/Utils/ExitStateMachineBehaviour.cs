using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitStateMachineBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SendMessage("OnEndedAnimation", stateInfo, SendMessageOptions.DontRequireReceiver);
    }
}
