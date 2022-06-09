using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKnifeAttackState : MagicKnifeState
{
    float attackTimer = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        knife.body.velocity = -knife.transform.up * knife.attackSpeed;
        attackTimer = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        attackTimer += Time.fixedDeltaTime;

        if(attackTimer >= knife.attackTime)
        {
            animator.SetTrigger("Spin");
        }
    }
}
