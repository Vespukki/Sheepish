using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInteractable
{
    public GameObject Prompt { get; }

    public void Interact(GameObject player);

    public void ShowPrompt();
    public void HidePrompt();

}
