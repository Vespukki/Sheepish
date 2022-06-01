using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHittable
{
    public string enemyName = "ERROR NO NAME";

    public void OnHit(PlayerMovement playerMover)
    {
    }
}
