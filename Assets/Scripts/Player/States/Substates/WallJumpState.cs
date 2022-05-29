using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : AirState
{
    public override void PhysicsUpdate(PlayerMovement mover)
    {
        //needs to be empty to cancel movement input;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.GetComponent<PlayerMovement>().WallJump();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerMovement>().StopCoroutine(animator.GetComponent<PlayerMovement>().WallJumpCut());
    }
}
