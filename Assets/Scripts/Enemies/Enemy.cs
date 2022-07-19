using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{
    public int damage;
    public int maxHealth;
    public int health;
    public Vector2 knockback;
    public Vector2 knockbackTaken;

    public bool knockable;
    public bool damageable;

    public string enemyName = "ERROR NO NAME";
    [HideInInspector] public GameObject target;
    [HideInInspector] public Vector2 targetDist;

    [HideInInspector] public Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        Initialize();

        health = maxHealth;
    }

    /// <summary>
    /// used instead of awake for children
    /// </summary>
    protected virtual void Initialize()
    {

    }

    private void OnEnable()
    {
        PlayerMovement.OnPlayerAwake += SetTarget;
    }
    private void OnDisable()
    {
        PlayerMovement.OnPlayerAwake -= SetTarget;
    }

    private void FixedUpdate()
    {
        //for target distance calculation
        Vector2 vec = transform.position - target.transform.position;
        targetDist = new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));

        //HurtboxCheck();
    }

    public virtual void OnHit(int damage, GameObject attacker)
    {
        health -= damage;

        if(health <= 0)
        {
            Destroy(gameObject);
        }

        if(knockable)
        {
            Knockback(knockbackTaken, Mathf.Sign(attacker.transform.position.x - transform.position.x));
        }
    }

    public void Knockback(Vector2 force, float direction)
    {
        body.velocity = Vector2.zero;
        body.AddForce(new Vector2(force.x * direction, force.y), ForceMode2D.Impulse);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponentInParent<PlayerHealth>().OnHit(damage, knockback, gameObject);
        }
    }

    void SetTarget(PlayerMovement mover)
    {
        target = mover.gameObject;
    }


}
