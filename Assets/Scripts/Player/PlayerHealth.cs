using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHittable
{
    [SerializeField] PlayerStats stats;
    
    public int health;
    [SerializeField] float invincibleTimer = 100;

    private void FixedUpdate()
    {
        invincibleTimer += Time.fixedDeltaTime;
    }

    public void OnHit(int damage, GameObject attacker)
    {
        if(invincibleTimer >= stats.invincibleTime)
        {
            Debug.Log("hit");
            TakeDamage(damage, attacker);
        }
    }

    void TakeDamage(int damage, GameObject attacker)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("dead rn");
        }

        invincibleTimer = 0;
    }
}
