using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RattadilloSleepingState : RattadilloState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if(rat.targetDist.x < rat.awakeDist.x && rat.targetDist.y < rat.awakeDist.y)
        {
            animator.SetTrigger("Awake");
        }
    }
}
