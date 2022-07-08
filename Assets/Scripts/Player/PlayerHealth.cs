using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHittable
{
    Animator animator;
    PlayerMovement mover;
    [SerializeField] PlayerStats stats;
    
    public int health;
    [SerializeField] float invincibleTimer = 100;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mover = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        invincibleTimer += Time.fixedDeltaTime;
    }

    public void OnHit(int damage, Vector2 knockback, GameObject attacker)
    {
        if(invincibleTimer >= stats.invincibleTime)
        {
            Debug.Log("player hit");
            TakeDamage(damage,knockback, attacker);
        }
    }

    void TakeDamage(int damage,Vector2 knockback, GameObject attacker)
    {
        animator.SetTrigger("Damaged");
        mover.Knockback(knockback, Mathf.Sign(transform.position.x - attacker.transform.position.x));

        health -= damage;

        if (health <= 0)
        {
            Debug.Log("dead rn");
        }

        invincibleTimer = 0;
    }
}
