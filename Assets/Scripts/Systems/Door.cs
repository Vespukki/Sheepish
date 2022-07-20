using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool useable = true;
    [SerializeField] Object targetScene;

    [SerializeField] Vector2 destination;



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInteraction inter) && useable)
        {
            inter.StartChangeScene(this, targetScene, destination);
        }
    }
}   
