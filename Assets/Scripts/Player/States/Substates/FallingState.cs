using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AirState
{
    public override void ModifyPhysics()
    {
        body.gravityScale = ((-2 * mover.stats.jumpHeight) / (mover.stats.jumpTime * mover.stats.jumpTime)) / -10 * mover.stats.fallSpeedMultiplier;
    }
}
