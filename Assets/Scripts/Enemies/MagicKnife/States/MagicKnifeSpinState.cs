using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKnifeSpinState : MagicKnifeState
{
    float spinTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        knife.body.velocity = Vector2.zero;
        spinTimer = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        spinTimer += Time.fixedDeltaTime;
        if(spinTimer >= knife.spinTime)
        {
            animator.SetTrigger("AttackStart");
        }
    }
}
