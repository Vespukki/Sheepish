using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKnifeStartAttackState : MagicKnifeState
{
    float startAttackTimer = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        knife.FaceTarget();
        startAttackTimer = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        startAttackTimer += Time.fixedDeltaTime;

        if(startAttackTimer >= knife.startAttackTime)
        {
            animator.SetTrigger("Attack");
        }
    }
}
