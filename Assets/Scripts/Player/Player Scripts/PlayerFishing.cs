using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerFishing : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioBank audioBank;
    [SerializeField] FishingStats fishingStats;

    public List<Fish> fishInventory = new();

    Fish fishWaiting = null;

    Coroutine fishWindow = null;
    Coroutine fishing = null;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        PlayerMovement.OnUnfish += UnfishHandler;
    }

    private void OnDestroy()
    {
        PlayerMovement.OnUnfish -= UnfishHandler;
    }

    public void StartFishing(Lure lure, FishTable table)
    {
        fishing = StartCoroutine(Fishing(lure, table));
    }

    IEnumerator Fishing(Lure lure, FishTable table)
    {
        yield return new WaitForSeconds(Random.Range(table.minTime, table.maxTime));
        Fish(lure, table);
    }

    public void Fish(Lure lure, FishTable table)
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
                fishWindow = StartCoroutine(FishWindow(lure, entry, table));
                break;
            }
        }
    }

    IEnumerator FishWindow(Lure lure, FishTableEntry entry, FishTable table)
    {
        PlayPing(fishingStats.rarityPings[entry.fish.rarity], fishingStats.rarityTimers[entry.fish.rarity], 2, lure.transform.position);
        audioSource.PlayOneShot(audioBank.fishPing[entry.fish.rarity]);

        fishWaiting = GetFish(entry.fish);
        yield return new WaitForSeconds(fishingStats.rarityTimers[entry.fish.rarity]);
        Debug.Log(entry.fish.name + " Got away");
        fishWaiting = null;
        StartFishing(lure, table);
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
        GameObject notif = Instantiate(fishingStats.notificationObject);
        FormatNotification(fish, notif.GetComponentInChildren<TextMeshProUGUI>());
        StartCoroutine(DestroyNotification(notif, fishingStats.notificationTimer));
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

    public void FormatNotification(Fish fish, TextMeshProUGUI textmesh)
    {
        textmesh.SetText(string.Format("{0} caught! Weight: {1}", fish.info.fishName, fish.weight));
    }

    void CatchFish(Fish fish)
    {
        fishInventory.Add(fish);
        Debug.Log(fish.info.fishName + " caught!");
        FishNotification(fish);
    }

    void UnfishHandler()
    {
        if (fishWaiting != null)
        {
            CatchFish(fishWaiting);
            fishWaiting = null;
        }

        if (fishWindow != null)
        {
            StopCoroutine(fishWindow);
        }
        if (fishing != null)
        {
            StopCoroutine(fishing);
        }
    }

    Fish GetFish(FishObject fish)
    {
        return new Fish(fish);
    }
}