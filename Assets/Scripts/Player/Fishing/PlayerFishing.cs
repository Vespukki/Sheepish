using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerFishing : MonoBehaviour
{
    PlayerMovement mover;
    Animator animator;
    AudioSource audioSource;
    [SerializeField] AudioBank audioBank;
    [SerializeField] FishingStats fishingStats;


    [SerializeField] CinemachineVirtualCamera lureCam;
    public List<Fish> fishInventory = new();

    Lure lureGlobal = null;

    //used as state check to see if you are currently catching something as well
    Fish fishWaiting = null;

    Coroutine fishWindow = null;
    Coroutine waitForFish = null;


    //true if unfish was called
    bool pulled;


    void Awake()
    {
        mover = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();


        PlayerMovement.OnUnfish += UnfishHandler;
    }

    private void OnDestroy()
    {
        PlayerMovement.OnUnfish -= UnfishHandler;
    }

    public void StartFishing(Lure lure, FishTable table)
    {
        lureCam.gameObject.SetActive(true);
        lureCam.Follow = lure.transform;
        pulled = false;
        lureGlobal = lure;
        animator.SetTrigger("Fish");
        Fishing(lure, table);
    }

    FishObject ChooseFish(FishTable table)
    {
        int totalParts = 0;
        foreach (FishTableEntry entry in table.fishes)
        {
            totalParts += entry.chancePart;
        }

        int chosenPart = Random.Range(0, totalParts);
        foreach (FishTableEntry entry in table.fishes)
        {
            totalParts -= entry.chancePart;
            if (totalParts <= chosenPart)
            {
                return entry.fish;
            }
        }
        Debug.LogError("ChooseFish failed, this is really bad and you should probably do something about it");
        return table.fishes[0].fish;
    }

    void Fishing(Lure lure, FishTable table)
    {
        waitForFish = StartCoroutine(WaitForFish(lure, ChooseFish(table), table, 0));
    }

    IEnumerator WaitForFish(Lure lure, FishObject fish, FishTable table, int currentRarity)
    {
        float timer = 0;
        float endTime = Random.Range(table.minTime, table.maxTime);
        pulled = false;
        lureCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = fishingStats.rarityShake[currentRarity];

        while (timer < endTime)
        {
            if(pulled)
            {
                pulled = false;
                EndFishing();
            }
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }

        fishWaiting = GetFish(fish);
        fishWindow = StartCoroutine(FishWindow(lure, fish, table, currentRarity));
    }

    IEnumerator FishWindow(Lure lure, FishObject fish, FishTable table, int currentRarity)
    {
        PlayPing(fishingStats.rarityPings[currentRarity], fishingStats.rarityTimers[currentRarity], 2, lure.transform.position);
        audioSource.PlayOneShot(audioBank.fishPing[currentRarity]);

        pulled = false;

        float timer = 0;
        while(timer < fishingStats.rarityTimers[currentRarity])
        {
            if(pulled)
            {
                if(currentRarity < fish.rarity)
                {
                    pulled = false;

                    lure.body.AddForce((transform.position - lure.transform.position) * Vector2.right);
                    waitForFish = StartCoroutine(WaitForFish(lure, fish, table, currentRarity + 1));
                    StopCoroutine(fishWindow);
                }
                else
                {
                    pulled = false;
                    CatchFish(fishWaiting);
                }
            }
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }

        //reaching here means fish got away
        fishWaiting = null;
        Fishing(lure, table);
    }

    void PlayPing(Sprite sprite, float lifetime, float pingSpeed, Vector2 position)
    {
        GameObject ping = Instantiate(fishingStats.pingObject, position, Quaternion.Euler(0,0,0));
        ping.GetComponent<SpriteRenderer>().sprite = sprite;
        ping.GetComponent<Rigidbody2D>().velocity = new Vector2(0, pingSpeed);
        StartCoroutine(DestroyPing(ping, lifetime));
    }

    IEnumerator DestroyPing(GameObject ping, float timer)
    {
        SpriteRenderer pingSpriter = ping.GetComponent<SpriteRenderer>();

        float currentTime = 0;
        while (currentTime < timer)
        {
            Color color = pingSpriter.color;
            color.a = 1 - Easing.SmoothStart3(currentTime / timer);
            pingSpriter.color = color;

            currentTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(ping);
        
    }

    void FishNotification(Fish fish)
    {
        GameObject notifObject = Instantiate(fishingStats.notificationObject);
        FishNotification notif = notifObject.GetComponent<FishNotification>();
        notif.FormatNotification(fish);
        StartCoroutine(DestroyNotification(notifObject, fishingStats.notificationTimer));
    }

    IEnumerator DestroyNotification(GameObject notif, float timer)
    {
        CanvasGroup notifCanvasGroup = notif.GetComponent<CanvasGroup>();

        float currentTime = 0;
        while (currentTime < timer)
        {
            notifCanvasGroup.alpha = 1 - Easing.SmoothStart3(currentTime / timer);

            currentTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(notif);

    }

    
    void CatchFish(Fish fish)
    {
        fishInventory.Add(fish);
        FishNotification(fish);
        EndFishing();
    }

    /// <summary>
    /// called by onUnfish event
    /// </summary>
    void UnfishHandler()
    {
        pulled = true;
        if(fishWaiting == null)
        {
            EndFishing();
        }
    }

    void EndFishing()
    {
        lureCam.Follow = null;
        lureCam.gameObject.SetActive(false);
        pulled = false;
        animator.SetTriggerOneFixedFrame(this, "FishCut");
        if(lureGlobal != null)
        {
            lureGlobal.body.AddForce(fishingStats.reelPower * (transform.position - lureGlobal.transform.position));
        }
    }

    /// <summary>
    /// only call this from lure.OnDelete
    /// </summary>
    public void StopFishing()
    {
        if (fishWindow != null)
        {
            StopCoroutine(fishWindow);
        }
        if(waitForFish != null)
        {
            StopCoroutine(waitForFish);
        }
    }

    Fish GetFish(FishObject fish)
    {
        return new Fish(fish);
    }
}