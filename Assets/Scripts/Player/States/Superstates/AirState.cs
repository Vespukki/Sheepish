using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : PlayerState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (!(mover.lastState is AirState))
        {
        }
    }
}
