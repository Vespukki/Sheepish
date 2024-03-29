using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : AirState
{
    public override void ModifyPhysics()
    {
        SetGravity(0);
    }
    public override void PhysicsUpdate()
    {
        //empty on purpose
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        body.constraints = RigidbodyConstraints2D.FreezePositionY;
        body.freezeRotation = true;

        mover.Dash();
        mover.remainingDashes--;

        body.velocity = new Vector2(body.velocity.x, 0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        body.constraints = RigidbodyConstraints2D.None;
        body.freezeRotation = true;

        body.velocity = new Vector2(mover.stats.speed * mover.lookingDir, body.velocity.y);
        mover.lastDashTimer = 0;
    }
}