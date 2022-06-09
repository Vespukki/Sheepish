using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{
    public string enemyName = "ERROR NO NAME";
    [HideInInspector] public GameObject target;
    [HideInInspector] public Vector2 targetDist;


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
    }

    public void OnHit(PlayerMovement playerMover)
    {

    }

    void SetTarget(PlayerMovement mover)
    {
        target = mover.gameObject;
    }
}
