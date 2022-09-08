using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingState : GroundedState
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        body.velocity = new Vector2(0, body.velocity.y);
    }
    public override void PhysicsUpdate()
    {
        
    }
}
