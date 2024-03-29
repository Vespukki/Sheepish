using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : StateMachineBehaviour
{
    protected PlayerMovement mover;
    protected Rigidbody2D body;
    protected PlayerInput input;
    protected PlayerInteraction inter;

    public bool canChangeDir = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mover = animator.GetComponent<PlayerMovement>();
        body = animator.GetComponent<Rigidbody2D>();
        input = animator.GetComponent<PlayerInput>();
        inter = animator.GetComponent<PlayerInteraction>();

        mover.currentState = this;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mover.lastState = this;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ModifyPhysics();
        PhysicsUpdate();
    }

    public virtual void ModifyPhysics()
    {
        SetGravity(1);
    }


    protected void SetGravity(float multiplier)
    {
        float waterMultiplier = mover.underwater ? mover.stats.waterGravityMultiplier : 1;

        body.gravityScale = mover.normalGravity * multiplier * waterMultiplier;
    }

    public virtual void PhysicsUpdate()
    {
        mover.Movement();
        mover.LimitFallSpeed(1);
    }
}
