using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : AirState
{
    public override void ModifyPhysics(PlayerMovement mover)
    {
        mover.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
    public override void PhysicsUpdate(PlayerMovement mover)
    {
        //empty on purpose
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.GetComponent<PlayerMovement>().Dash();
        animator.GetComponent<PlayerMovement>().remainingDashes--;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        animator.GetComponent<PlayerMovement>().lastDashTimer = 0;
    }
}