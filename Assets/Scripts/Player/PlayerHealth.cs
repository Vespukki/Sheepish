using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    Animator animator;
    PlayerMovement mover;
    Rigidbody2D body;
    PlayerInteraction inter;

    [SerializeField] PlayerStats stats;
    
    public int health;
    [SerializeField] float invincibleTimer = 100;

    [HideInInspector] public Vector2 respawnAnchor;
    [HideInInspector] public int respawnSceneIndex = 1;

    [SerializeField] Canvas deathScreen;

    private void Awake()
    {   
        animator = GetComponent<Animator>();
        mover = GetComponent<PlayerMovement>();
        body = GetComponent<Rigidbody2D>();
        inter = GetComponent<PlayerInteraction>();

        respawnAnchor = new Vector2(transform.position.x, transform.position.y);
    }

    private void FixedUpdate()
    {
        invincibleTimer += Time.fixedDeltaTime;
    }

    public void OnHit(int damage, Vector2 knockback, GameObject attacker)
    {
        if(invincibleTimer >= stats.invincibleTime)
        {
            TakeDamage(damage,knockback, attacker);
        }
    }

    void TakeDamage(int damage,Vector2 knockback, GameObject attacker)
    {
        mover.Knockback(knockback, Mathf.Sign(transform.position.x - attacker.transform.position.x));

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Damaged");
        }

        invincibleTimer = 0;
    }

    void Die()
    {
        OpenDeathScreen();
        animator.SetTrigger("Reset");
        body.velocity = Vector2.zero;
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
        inter.ClearInteractTarget();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        deathScreen.gameObject.SetActive(false);

        yield return StartCoroutine(Easing.ScreenFadeOut());

        AsyncOperation unloadProgress = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(currentSceneIndex));
        while(!unloadProgress.isDone)
        {
            yield return null;
        }
        AsyncOperation loadProgress = SceneManager.LoadSceneAsync(respawnSceneIndex, LoadSceneMode.Additive);
        while(!loadProgress.isDone)
        {
            yield return null;
        }
        

        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(respawnSceneIndex));


        Time.timeScale = 1;
        transform.position = respawnAnchor;
        StartCoroutine(Easing.ScreenFadeIn());
        mover.CallPlayerAwake();
    }

    public IEnumerator StartGame(int startSceneIndex)
    {
        Easing.SetBlackScreen();
        Time.timeScale = 0;
        AsyncOperation loadProgress = SceneManager.LoadSceneAsync(startSceneIndex, LoadSceneMode.Additive);
        while (!loadProgress.isDone)
        {
            yield return null;
        }

        Time.timeScale = 1;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(startSceneIndex));
        StartCoroutine(Easing.ScreenFadeIn());
    }
}
