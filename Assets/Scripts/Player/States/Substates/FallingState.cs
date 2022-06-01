using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AirState
{
    public override void ModifyPhysics(PlayerMovement mover)
    {
        mover.GetComponent<Rigidbody2D>().gravityScale = ((-2 * mover.stats.jumpHeight) / (mover.stats.jumpTime * mover.stats.jumpTime)) / -10 * mover.stats.fallSpeedMultiplier;
    }
}
