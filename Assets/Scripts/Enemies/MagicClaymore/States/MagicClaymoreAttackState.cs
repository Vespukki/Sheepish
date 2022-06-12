using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicClaymoreAttackState : MagicClaymoreState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        claymore.body.velocity = Vector2.zero;
    }
}
