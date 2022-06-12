using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicClaymore : Enemy
{
    [HideInInspector] public MagicClaymoreState currentState;

    [HideInInspector] public Rigidbody2D body;

    public Vector2 aggroDist;
    public Vector2 attackDist;
    [SerializeField] float walkSpeed;
    [SerializeField] float walkAccel;
    [SerializeField] float walkDeccel;
    [SerializeField] float turnTime;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }


    public void FaceTarget(GameObject walkTarget)
    {
            transform.localScale = new Vector3(-Mathf.Sign(walkTarget.transform.position.x - transform.position.x), 1, 1);
    }

    public void Walk(GameObject walkTarget)
    {
        float targetSpeed = Mathf.Sign(walkTarget.transform.position.x - transform.position.x) * walkSpeed; //dir to move in at speed
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
