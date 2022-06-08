using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClingState : GroundedState
{
    public override void ModifyPhysics()
    {
        SetGravity(0);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        float targetSpeed = -mover.stats.wallClingFallSpeed;
        float speedDiff = targetSpeed - body.velocity.y;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * mover.stats.wallClingAcceleration, mover.stats.jerk) * Mathf.Sign(speedDiff);

        body.AddForce(movement * Vector2.up);
    }
}
