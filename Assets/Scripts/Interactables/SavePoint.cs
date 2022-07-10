using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SavePoint : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject prompt;
    public GameObject Prompt => prompt;

    public void Interact(GameObject player)
    {
        Debug.Log("Save not implemented but it was interacted with");
    }
    public void ShowPrompt()
    {
        Prompt.gameObject.SetActive(true);
    }
    public void HidePrompt()
    {
        Prompt.gameObject.SetActive(false);
    }
}
