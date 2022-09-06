using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{

    LineRenderer liner;

    [HideInInspector] public Rigidbody2D body;
    [HideInInspector] public DistanceJoint2D distJoint;
    [HideInInspector] public AudioSource audioSource;

    [HideInInspector] public lureState state = lureState.air;

    // set by player mover on instantiate
    [HideInInspector] public PlayerMovement mover;
    [HideInInspector] public PlayerFishing fisher;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        liner = GetComponent<LineRenderer>();
        distJoint = GetComponent<DistanceJoint2D>();
        audioSource = GetComponent<AudioSource>();

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
        if(collision.collider.CompareTag("Ground") && state == lureState.air)
        {
            Grapple();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Water water) && state == lureState.air)
        {
            Fish(water.fishTable);
        }
    }

    void Fish(FishTable fishTable)
    {
        state = lureState.fishing;
        fisher.StartFishing(this, fishTable);
    }

    void Grapple()
    {
        state = lureState.grappling;

        distJoint.connectedBody = mover.gameObject.GetComponent<Rigidbody2D>();
        distJoint.distance = Vector2.Distance(transform.position, mover.transform.position);
        distJoint.enabled = true;

        body.bodyType = RigidbodyType2D.Static;
        mover.GetComponent<Animator>().SetTrigger("Grapple");
    }
}
public enum lureState { fishing, grappling, air }
