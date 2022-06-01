using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDrillAttackState : AirState
{
    public override void ModifyPhysics(PlayerMovement mover)
    {
        mover.GetComponent<Rigidbody2D>().gravityScale = 0;
        mover.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public override void PhysicsUpdate(PlayerMovement mover)
    {
        mover.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
