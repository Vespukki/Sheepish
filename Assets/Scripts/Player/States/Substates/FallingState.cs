using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AirState
{
    public override void ModifyPhysics()
    {
        SetGravity(mover.stats.fallSpeedMultiplier);
    }
}
