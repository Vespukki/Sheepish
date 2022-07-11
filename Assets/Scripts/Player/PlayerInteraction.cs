using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    IInteractable interactTarget;


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
}
