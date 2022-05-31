using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRock : MonoBehaviour, IHittable
{
    public void OnHit(PlayerMovement playerMover)
    {
        Debug.Log("target rock hit");
    }
}
