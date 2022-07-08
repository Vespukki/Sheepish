using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKnifeRisingState : MagicKnifeState
{
    float riseTimer = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        riseTimer = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        knife.body.AddForce(knife.riseSpeed * new Vector2(-knife.transform.right.x, Mathf.Abs(knife.transform.right.y)),ForceMode2D.Impulse);
        knife.FaceTarget();

        riseTimer += Time.fixedDeltaTime;
        if(riseTimer >= knife.riseTime)
        {
            animator.SetTrigger("Spin");
        }
    }
}
