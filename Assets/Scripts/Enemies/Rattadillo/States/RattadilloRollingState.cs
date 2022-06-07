using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RattadilloRollingState : RattadilloState
{
    protected override void OnWallHit(Animator animator)
    {
        base.OnWallHit(animator);

        animator.SetTrigger("Warning");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        rat.Movement(rat.rollSpeed, rat.rollAcceleration, rat.rollDecceleration);
    }

}
