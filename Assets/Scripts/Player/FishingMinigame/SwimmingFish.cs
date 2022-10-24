using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingFish : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer spriter;
    [SerializeField] float speed;
    bool goingRight = true;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        body.velocity = speed * Vector2.right;
        FaceDirection();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Wall"))
        {
            body.velocity = new Vector2(speed * (goingRight ? -1 : 1), 0 );
            goingRight = !goingRight;

            FaceDirection();
        }
    }

    void FaceDirection()
    {
        if(goingRight)
        {
            spriter.flipX = true;
        }
        else
        {
            spriter.flipX = false;
        }
    }
}
