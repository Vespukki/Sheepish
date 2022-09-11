using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool useable = true;
    [SerializeField] Object targetScene;
    
    public SceneTransition transition = SceneTransition.walking;

    private void Start()
    {
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        useable = false;
        yield return new WaitForSeconds(.25f);
        useable = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInteraction inter) && useable)
        {
            inter.StartChangeScene(this, targetScene);
        }
    }
}   

public enum SceneTransition { walking, falling, jumping}
