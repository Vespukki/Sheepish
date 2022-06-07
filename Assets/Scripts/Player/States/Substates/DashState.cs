using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : AirState
{
    public override void ModifyPhysics()
    {
        mover.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
    public override void PhysicsUpdate()
    {
        //empty on purpose
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        mover.Dash();
        mover.remainingDashes--;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        mover.lastDashTimer = 0;
    }
}