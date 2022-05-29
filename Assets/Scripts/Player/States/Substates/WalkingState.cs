using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : GroundedState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.GetComponent<PlayerMovement>().currentState = this;
    }
}
