using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillAttackState : AirState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.GetComponent<PlayerMovement>().StartAttack(animator.GetComponent<PlayerMovement>().drillAttackCollider);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        animator.GetComponent<PlayerMovement>().EndAttack(animator.GetComponent<PlayerMovement>().drillAttackCollider);
    }

    public override void ModifyPhysics(PlayerMovement mover)
    {
        mover.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public override void PhysicsUpdate(PlayerMovement mover)
    {
        float targetSpeed = -mover.stats.drillSpeed;
        float speedDiff = targetSpeed - mover.GetComponent<Rigidbody2D>().velocity.y;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * mover.stats.wallClingAcceleration, 1) * Mathf.Sign(speedDiff);

        mover.GetComponent<Rigidbody2D>().AddForce(movement * Vector2.up);
    }
}
