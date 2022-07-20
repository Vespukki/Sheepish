using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    IInteractable interactTarget;

    PlayerMovement mover;
    private void Awake()
    {
        mover = GetComponent<PlayerMovement>();
    }

    public void TryInteract()
    {
        interactTarget?.Interact(gameObject);
    }

    private void FixedUpdate()
    {
        interactTarget = GetInteractionTarget();
    }

    IInteractable GetInteractionTarget()
    {
        float dist = 100;
        IInteractable closest = null;
        foreach(var col in Physics2D.OverlapBoxAll(transform.position, new Vector2(2,2),0))
        {
            if(col.TryGetComponent<IInteractable>(out IInteractable iInt))
            {
                float newDist = Vector2.Distance(transform.position, col.transform.position);

                if(newDist < dist)
                {
                    closest = iInt;
                    dist = newDist;
                }
            }
        }

        if(interactTarget != closest)
        {
            interactTarget?.HidePrompt();
            closest?.ShowPrompt();
        }
        return closest;
    }

    public void ClearInteractTarget()
    {
        interactTarget?.HidePrompt();
        interactTarget = null;
    }

    IEnumerator ChangeScene(Door door, Object targetScene, Vector2 destination)
    {
        Time.timeScale = 0;
        yield return StartCoroutine(Easing.ScreenFadeOut());
        
        ClearInteractTarget();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        AsyncOperation unloadProgress = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(currentSceneIndex));
        while (!unloadProgress.isDone)
        {
            yield return null;
        }
        AsyncOperation loadProgress = SceneManager.LoadSceneAsync(targetScene.name, LoadSceneMode.Additive);
        while (!loadProgress.isDone)
        {
            yield return null;
        }


        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene.name));

        Time.timeScale = 1;
        transform.position = destination;
        StartCoroutine(Easing.ScreenFadeIn());
        mover.CallPlayerAwake();
    }

    public void StartChangeScene(Door door, Object targetScene, Vector2 destination)
    {
        StartCoroutine(ChangeScene(door, targetScene, destination));
    }
}