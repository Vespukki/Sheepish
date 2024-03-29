using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicClaymore : Enemy
{
    [HideInInspector] public MagicClaymoreState currentState;


    public Vector2 aggroDist;
    public Vector2 attackDist;
    [SerializeField] float walkSpeed;
    [SerializeField] float walkAccel;
    [SerializeField] float walkDeccel;
    [SerializeField] float turnTime;


    public void FaceTarget(Transform walkTarget)
    {
            transform.localScale = new Vector3(-Mathf.Sign(walkTarget.position.x - transform.position.x), transform.localScale.y, 1);
    }

    public void Walk(Transform walkTarget, bool stop = false)
    {
        float targetSpeed = 0;
        if(stop == false)
        {
             targetSpeed = Mathf.Sign(walkTarget.position.x - transform.position.x) * walkSpeed; //dir to move in at speed
        }

        float speedDiff = targetSpeed - body.velocity.x; //diff between current and desired

        float accelRate = (Mathf.Abs(targetSpeed) > .01) ? walkAccel : walkDeccel; //choose to accelerate or decelerate

        //applies acceleration to speedDiff, then sets to a power to set jerk. then reapplies direction
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 1) * Mathf.Sign(speedDiff);

        body.AddForce(movement * Vector2.right);

        //no small numbers!
        if (Mathf.Abs(body.velocity.x) < 0.05)
        {
            body.velocity = new Vector2(0, body.velocity.y);
        }
    }
}
