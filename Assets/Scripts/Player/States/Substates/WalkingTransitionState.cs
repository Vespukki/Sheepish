using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingTransitionState : GroundedState
{
    public override void PhysicsUpdate()
    {
        mover.ForcedMove(mover.lookingDir);
        mover.LimitFallSpeed(1);
    }
}
