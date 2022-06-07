using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : StateMachineBehaviour
{
    protected PlayerMovement mover;
    protected Rigidbody2D body;

    public bool canChangeDir = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mover = animator.GetComponent<PlayerMovement>();
        body = animator.GetComponent<Rigidbody2D>();

        mover.currentState = this;
        ModifyPhysics();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mover.lastState = this;
    }

    public virtual void ModifyPhysics()
    {
        mover.gameObject.GetComponent<Rigidbody2D>().gravityScale = ((-2 * mover.stats.jumpHeight) / (mover.stats.jumpTime * mover.stats.jumpTime)) / -10;
    }
    public virtual void PhysicsUpdate()
    {
        mover.Movement();
        mover.LimitFallSpeed();
    }
}
