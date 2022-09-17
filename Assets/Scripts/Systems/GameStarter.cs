using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField] int startSceneIndex;
    PlayerHealth health;


    private void Start()
    {
        health = GetComponent<PlayerHealth>();
        health.StartCoroutine(health.StartGame(startSceneIndex));
        Destroy(this);
    }
}
