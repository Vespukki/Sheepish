using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TalkingState : GroundedState
{
    InputActionMap previousMap;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        body.velocity = Vector2.zero;

        previousMap = input.currentActionMap;
        input.SwitchCurrentActionMap("Dialog");

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        input.SwitchCurrentActionMap(previousMap.name);
    }

    public override void PhysicsUpdate()
    {
        //empty on purpose
    }
}
