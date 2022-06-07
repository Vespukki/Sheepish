using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{
    public string enemyName = "ERROR NO NAME";

    public void OnHit(PlayerMovement playerMover)
    {

    }

    private void FixedUpdate()
    {
        AI();
    }
    public abstract void AI();
}
