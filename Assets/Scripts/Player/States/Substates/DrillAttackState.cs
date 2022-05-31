using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillAttackState : AirState
{
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
