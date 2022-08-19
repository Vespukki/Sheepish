using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject prompt;
    public GameObject Prompt => prompt;

    [SerializeField] Dialog[] dialogs;

    public void Interact(GameObject player)
    {
        player.GetComponent<Animator>().SetTrigger("Talk");

        player.GetComponent<DialogHandler>().StartDialog(dialogs, this);
    }
}