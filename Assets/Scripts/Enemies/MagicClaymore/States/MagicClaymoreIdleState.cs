using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicClaymoreIdleState : MagicClaymoreState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        claymore.body.velocity = Vector2.zero;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        claymore.Walk(claymore.transform, true);

        if (claymore.targetDist.x <= claymore.aggroDist.x && claymore.targetDist.y <= claymore.aggroDist.y)
        {
            animator.SetTrigger("Walk");
        }
    }
}
