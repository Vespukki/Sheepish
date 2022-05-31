using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDownAttackState : AirState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.GetComponent<PlayerMovement>().StartAttack(animator.GetComponent<PlayerMovement>().downAttackCollider);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        animator.GetComponent<PlayerMovement>().EndAttack(animator.GetComponent<PlayerMovement>().downAttackCollider);
    }
    public override void ModifyPhysics(PlayerMovement mover)
    {
        base.ModifyPhysics(mover);
    }
}
