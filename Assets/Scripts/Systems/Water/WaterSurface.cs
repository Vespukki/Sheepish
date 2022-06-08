using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    [HideInInspector] public PlatformEffector2D platform;
    [HideInInspector] public Collider2D coll;

    private void Awake()
    {
        platform = GetComponent<PlatformEffector2D>();
        coll = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
         PlayerMovement.OnPlayerAwake += PlayerAwake;
    }
    private void OnDisable()
    {
         PlayerMovement.OnPlayerAwake -= PlayerAwake;
    }


    void PlayerAwake(PlayerMovement mover)
    {

    }
}
