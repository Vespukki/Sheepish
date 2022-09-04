using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{
    Rigidbody2D body;
    LineRenderer liner;
    [HideInInspector] public DistanceJoint2D distJoint;

    [HideInInspector] public lureState state = lureState.air; 

    [HideInInspector] public PlayerMovement mover; // set by player mover on instantiate

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        liner = GetComponent<LineRenderer>();
        distJoint = GetComponent<DistanceJoint2D>();

        distJoint.enabled = false;
        body.gravityScale = PlayerMovement.playerGravity;
    }

    private void Update()
    {
       liner.SetPosition(0, mover.transform.position);
       liner.SetPosition(1, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        distJoint.connectedBody = mover.gameObject.GetComponent<Rigidbody2D>();
        distJoint.distance = Vector2.Distance(transform.position, mover.transform.position);
        distJoint.enabled = true;

        //mover.canMove = false;

        body.bodyType = RigidbodyType2D.Static;
        state = lureState.grappling;
        mover.GetComponent<Animator>().SetTrigger("Grapple");
    }

    private void OnDestroy()
    {
    }
}
public enum lureState { fishing, grappling, air }
