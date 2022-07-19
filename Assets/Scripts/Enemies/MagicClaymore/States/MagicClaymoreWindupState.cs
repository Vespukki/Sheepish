using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicClaymoreWindupState : MagicClaymoreState
{
    bool knockableMemory;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        knockableMemory = claymore.knockable;

        claymore.knockable = false;
        claymore.body.velocity = Vector2.zero;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        claymore.knockable = knockableMemory;
    }
}
