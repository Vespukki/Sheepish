using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RattadilloWarningState : RattadilloState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (rat.targetDist.x < rat.rollingDist.x && rat.targetDist.y < rat.rollingDist.y)
        {
            animator.SetTrigger("Rolling");
        }
    }
}
