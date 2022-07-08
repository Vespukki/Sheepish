using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKnifeState : StateMachineBehaviour
{
    protected MagicKnife knife;
    public bool knockable;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        knife = animator.GetComponent<MagicKnife>();
        knife.knockable = knockable;    
        knife.currentState = this;
    }
}
