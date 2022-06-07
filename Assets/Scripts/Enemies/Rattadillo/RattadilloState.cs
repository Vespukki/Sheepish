using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RattadilloState : StateMachineBehaviour
{
    protected Rattadillo rat;
    protected Rigidbody2D body;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rat = animator.GetComponent<Rattadillo>();
        body = animator.GetComponent<Rigidbody2D>();
        rat.OnHitWall += OnWallHit;
        rat.currentState = this;
        OnStateUpdate(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        rat.OnHitWall -= OnWallHit;

    }

    protected virtual void OnWallHit(Animator animator)
    {
            rat.Flip();
    }

    protected void FacePlayer()
    {
        if (Mathf.Sign(rat.target.transform.position.x - rat.transform.position.x) * rat.facingDir < 0)//if target is on the left of rat * the facing direction, so facing opposite the player
        {
            rat.Flip();
        }
    }
}
