using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IHittable
{
    Animator animator;
    PlayerMovement mover;
    [SerializeField] PlayerStats stats;
    
    public int health;
    [SerializeField] float invincibleTimer = 100;

    [SerializeField] Canvas deathScreen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mover = GetComponent<PlayerMovement>();
    }


    private void FixedUpdate()
    {
        invincibleTimer += Time.fixedDeltaTime;
    }

    public void OnHit(int damage, Vector2 knockback, GameObject attacker)
    {
        if(invincibleTimer >= stats.invincibleTime)
        {
            Debug.Log("player hit");
            TakeDamage(damage,knockback, attacker);
        }
    }

    void TakeDamage(int damage,Vector2 knockback, GameObject attacker)
    {
        animator.SetTrigger("Damaged");
        mover.Knockback(knockback, Mathf.Sign(transform.position.x - attacker.transform.position.x));

        health -= damage;

        if (health <= 0)
        {
            OpenDeathScreen();
        }

        invincibleTimer = 0;
    }

    void OpenDeathScreen()
    {
        Time.timeScale = 0;
        deathScreen.gameObject.SetActive(true);
    }

    public void RespawnButton()
    {
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        /*Scene newScene = SceneManager.GetActiveScene();
        AsyncOperation loadProgress = SceneManager.LoadSceneAsync(newScene.name, LoadSceneMode.Additive);
        AsyncOperation unloadProgress = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        unloadProgress.allowSceneActivation = false;
        loadProgress.allowSceneActivation = false;
        if(!unloadProgress.isDone || !loadProgress.isDone)
        {
            yield return null;
        }
        unloadProgress.allowSceneActivation = true;
        loadProgress.allowSceneActivation = true;
        SceneManager.SetActiveScene(newScene);
*/
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation loadProgress = SceneManager.LoadSceneAsync(SceneManager.GetSceneByBuildIndex(sceneIndex).name, LoadSceneMode.Additive);
        if (!loadProgress.isDone)
        {
            yield return null;
        }
        AsyncOperation unloadProgress = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(sceneIndex));
        if(!unloadProgress.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
        Time.timeScale = 1;
        deathScreen.gameObject.SetActive(false);
        mover.CallPlayerAwake();
    }
}
