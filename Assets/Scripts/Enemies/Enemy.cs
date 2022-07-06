using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{
    public int damage;
    public int health;

    public string enemyName = "ERROR NO NAME";
    [HideInInspector] public GameObject target;
    [HideInInspector] public Vector2 targetDist;

    [SerializeField] Collider2D hitbox;

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
        Debug.Log(damage.ToString() + " damage taken");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponentInParent<IHittable>().OnHit(damage, gameObject);
        }
    }
    void HurtboxCheck()
    {
        if (hitbox == null) return;


        List<Collider2D> colliders = new List<Collider2D>();

        hitbox.GetContacts(colliders);

        foreach (var hit in colliders)
        {
            if (hit.IsTouching(GetComponent<Collider2D>()) && hit.CompareTag("Player") && hit.TryGetComponent(out IHittable victim))
            {
                victim.OnHit(damage, gameObject);
                return;
            }
        }
    }

    void SetTarget(PlayerMovement mover)
    {
        target = mover.gameObject;
    }


}
