using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAttackKnockbackState : AirState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.GetComponent<PlayerMovement>().knockbackTimer = 0;
    }

    public override void PhysicsUpdate(PlayerMovement mover)
    {
        if (mover.knockbackTimer >= mover.stats.attackKnockbackTime)
        {
            mover.GetComponent<Animator>().SetTrigger("KnockbackCut");
        }
    }
}
