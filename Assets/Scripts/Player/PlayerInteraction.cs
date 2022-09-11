using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    IInteractable interactTarget;

    PlayerMovement mover;
    Animator animator;

    Dictionary<SceneTransition, string> transitionDict = new()
    {
        [SceneTransition.walking] = "WalkingTransition",
        [SceneTransition.falling] = "FallingTransition",
        [SceneTransition.jumping] = "JumpingTransition"
    };
    private void Awake()
    {
        mover = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
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

    IEnumerator ChangeScene(Door door, Object targetScene)
    {
        animator.SetTrigger(transitionDict[door.transition]);
        yield return StartCoroutine(Easing.ScreenFadeOut());
        Time.timeScale = 0;

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
        StartCoroutine(Easing.ScreenFadeIn());
        mover.CallPlayerAwake();
    }

    public void StartChangeScene(Door door, Object targetScene)
    {
        StartCoroutine(ChangeScene(door, targetScene));
    }
}