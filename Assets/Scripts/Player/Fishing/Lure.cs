using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{
    Rigidbody2D body;
    LineRenderer liner;
    TargetJoint2D targetJoint;

    [HideInInspector] public PlayerMovement mover; // set by player mover on instantiate

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        liner = GetComponent<LineRenderer>();
        targetJoint = GetComponent<TargetJoint2D>();

        targetJoint.enabled = false;
        body.gravityScale = PlayerMovement.playerGravity;
    }

    private void Update()
    {
       liner.SetPosition(0, mover.transform.position);
       liner.SetPosition(1, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        targetJoint.target = collision.GetContact(0).point;
        targetJoint.enabled = true;

    }
}
