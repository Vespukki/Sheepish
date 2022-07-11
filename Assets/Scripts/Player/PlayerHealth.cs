using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IHittable
{
    Animator animator;
    PlayerMovement mover;
    Rigidbody2D body;

    [SerializeField] PlayerStats stats;
    
    public int health;
    [SerializeField] float invincibleTimer = 100;

    public Vector2 respawnAnchor;
    public int respawnSceneIndex;

    [SerializeField] Canvas deathScreen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mover = GetComponent<PlayerMovement>();
        body = GetComponent<Rigidbody2D>();

        respawnAnchor = new Vector2(transform.position.x, transform.position.y);
        respawnSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
        mover.Knockback(knockback, Mathf.Sign(transform.position.x - attacker.transform.position.x));

        health -= damage;

        if (health <= 0)
        {
            OpenDeathScreen();
            animator.SetTrigger("Reset");
            body.velocity = Vector2.zero;
        }
        else
        {
            animator.SetTrigger("Damaged");
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation loadProgress = SceneManager.LoadSceneAsync(respawnSceneIndex, LoadSceneMode.Additive);
        if (!loadProgress.isDone)
        {
            yield return null;
        }
        AsyncOperation unloadProgress = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(currentSceneIndex));
        if(!unloadProgress.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(respawnSceneIndex));
        Time.timeScale = 1;
        deathScreen.gameObject.SetActive(false);
        transform.position = respawnAnchor;
        mover.CallPlayerAwake();
    }
}
