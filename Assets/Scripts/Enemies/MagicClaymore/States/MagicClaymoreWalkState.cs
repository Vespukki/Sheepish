using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicClaymoreWalkState : MagicClaymoreState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        claymore.Walk(claymore.target);
        claymore.FaceTarget(claymore.target);

        if (claymore.targetDist.x <= claymore.attackDist.x && claymore.targetDist.y <= claymore.attackDist.y)
        {
            animator.SetTrigger("Attack");
        }
    }
}
