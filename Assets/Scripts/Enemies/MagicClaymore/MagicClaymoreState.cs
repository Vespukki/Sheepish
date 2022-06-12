using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicClaymoreState : StateMachineBehaviour
{
    protected MagicClaymore claymore;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        claymore = animator.GetComponent<MagicClaymore>();

        claymore.currentState = this;
    }
}
