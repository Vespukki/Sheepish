using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDrillAttackState : AirState
{
    public override void ModifyPhysics()
    {
        SetGravity(0);
        body.velocity = Vector2.zero;
    }

    public override void PhysicsUpdate()
    {
        body.velocity = Vector2.zero;
    }
}
