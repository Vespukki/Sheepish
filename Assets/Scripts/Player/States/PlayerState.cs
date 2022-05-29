using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : StateMachineBehaviour
{
    protected static PlayerState lastState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerMovement>().currentState = this;
        ModifyPhysics(animator.GetComponent<PlayerMovement>());
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lastState = this;
    }

    public virtual void ModifyPhysics(PlayerMovement mover)
    {
        mover.gameObject.GetComponent<Rigidbody2D>().gravityScale = ((-2 * mover.stats.jumpHeight) / (mover.stats.jumpTime * mover.stats.jumpTime)) / -10;
    }
    public virtual void PhysicsUpdate(PlayerMovement mover)
    {
        mover.Movement();
        mover.LimitFallSpeed();
    }
}
