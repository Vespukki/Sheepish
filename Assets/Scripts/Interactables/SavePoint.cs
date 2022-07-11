using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SavePoint : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject prompt;
    public GameObject Prompt => prompt;

    private void Awake()
    {
        prompt = transform.Find("Prompt").gameObject;
    }

    public void Interact(GameObject player)
    {
        player.GetComponent<PlayerHealth>().respawnAnchor = new Vector2(transform.position.x, transform.position.y);
    }
}
