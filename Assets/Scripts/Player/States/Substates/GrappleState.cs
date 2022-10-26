using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleState : AirState
{
    public override void PhysicsUpdate()
    {
        mover.GrappleSwing();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        body.drag = 0;

        mover.DeleteLure();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }
}
