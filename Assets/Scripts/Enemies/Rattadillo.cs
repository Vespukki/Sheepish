using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rattadillo : Enemy
{
    int facingDir = 1;
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;
 
    [SerializeField] Collider2D frontWallCollider;

    Rigidbody2D body;
    Animator animator;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public override void AI()
    {
        WallCheck();
        animator.SetInteger("FacingDir", facingDir);
        Movement();
    }

    void WallCheck()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        frontWallCollider.GetContacts(colliders);

        foreach (var hit in colliders)
        {
            if (hit.IsTouching(GetComponent<Collider2D>()) && hit.CompareTag("Ground"))
            {
                facingDir *= -1;
                transform.localScale = new Vector3(facingDir, 1, 1);
                return;
            }
        }
    }

    void Movement()
    {
        float targetSpeed = facingDir * speed; //dir to move in at speed
        float speedDiff = targetSpeed - body.velocity.x; //diff between current and desired

        float accelRate = (Mathf.Abs(targetSpeed) > .01) ? acceleration : decceleration; //choose to accelerate or decelerate

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