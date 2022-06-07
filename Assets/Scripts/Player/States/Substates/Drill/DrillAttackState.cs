using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillAttackState : AirState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        mover.StartAttack(mover.drillAttackCollider);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        mover.EndAttack(mover.drillAttackCollider);
    }

    public override void ModifyPhysics()
    {
       body.gravityScale = 0;
    }

    public override void PhysicsUpdate()
    {
        float targetSpeed = -mover.stats.drillSpeed;
        float speedDiff = targetSpeed - body.velocity.y;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * mover.stats.wallClingAcceleration, 1) * Mathf.Sign(speedDiff);

        body.AddForce(movement * Vector2.up);
    }
}
