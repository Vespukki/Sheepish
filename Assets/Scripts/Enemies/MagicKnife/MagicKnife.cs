using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKnife : Enemy
{
    [HideInInspector] public MagicKnifeState currentState;

    Animator animator;
    
    public Vector2 aggroDist;
    public float idleTime;
    public float riseSpeed;
    public float riseTime;
    public float spinTime;
    public float startAttackTime;
    public float attackTime;
    public float attackSpeed;

    public Transform embedPoint;

    [HideInInspector] public Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public void FaceTarget()
    {
        transform.up = -(target.transform.position - transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(currentState is MagicKnifeAttackState)
        {
            if(collision.CompareTag("Ground"))
            {
                animator.SetTrigger("Idle");
            }
        }
    }
}
