using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rattadillo : Enemy
{
    [HideInInspector] public int facingDir = 1;
    [HideInInspector] public RattadilloState currentState;

    [Header("Rolling")]
    public float rollSpeed;
    public float rollAcceleration;
    public float rollDecceleration;

    [Header("Logic")]
    public Vector2 awakeDist = new Vector2();
    public Vector2 warningDist = new Vector2();
    public Vector2 rollingDist = new Vector2();

    [SerializeField] Collider2D frontWallCollider;

    Rigidbody2D body;
    Animator animator;

    //events that are referenced by mechanim
    public delegate void HitWall(Animator animator);
    public event HitWall OnHitWall;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        WallCheck();
        
    }

    void WallCheck()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        frontWallCollider.GetContacts(colliders);

        foreach (var hit in colliders)
        {
            if (hit.IsTouching(GetComponent<Collider2D>()) && hit.CompareTag("Ground"))
            {
                if(OnHitWall != null)
                {
                    OnHitWall(animator);
                }
                return;
            }
        }
    }

    public void Movement(float speed, float acceleration, float decceleration)
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

    public void Flip()
    {
        facingDir *= -1;
        transform.localScale = new Vector3(facingDir, 1, 1);
    }
}