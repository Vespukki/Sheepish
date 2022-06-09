using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKnifeIdleState : MagicKnifeState
{
    float idleTimer = 100;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        knife.body.velocity = Vector2.zero;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        idleTimer += Time.fixedDeltaTime;

        if(idleTimer >= knife.idleTime)
        {
            if (knife.targetDist.x < knife.aggroDist.x && knife.targetDist.y < knife.aggroDist.y)
            {
                animator.SetTrigger("Rising");
            }
        }
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        idleTimer = 0;
    }
}
