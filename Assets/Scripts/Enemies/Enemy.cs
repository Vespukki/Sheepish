using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{
    public string enemyName = "ERROR NO NAME";
    [HideInInspector] public GameObject target;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerAwake += SetTarget;
    }
    private void OnDisable()
    {
        PlayerMovement.OnPlayerAwake -= SetTarget;
    }

    public void OnHit(PlayerMovement playerMover)
    {

    }

    void SetTarget(GameObject player)
    {
        target = player;
    }
}
