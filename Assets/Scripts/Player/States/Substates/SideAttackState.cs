using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAttackState : AirState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        mover.StartAttack(animator.GetComponent<PlayerMovement>().downAttackCollider);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        mover.EndAttack(animator.GetComponent<PlayerMovement>().downAttackCollider);
    }
}
