using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] FishingStats fishingStats;
    public Dictionary<FishObject, bool> hasCaught = new();

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject container;

    List<GameObject> rarityTierBoxes = new();

    private void OnDestroy()
    {
        PlayerMovement.OnInventoryOpen -= OpenInventory;
        PlayerMovement.OnInventoryClose -= CloseInventory;
    }

    private void Awake()
    {
        PlayerMovement.OnInventoryOpen += OpenInventory;
        PlayerMovement.OnInventoryClose += CloseInventory;


        foreach (var fish in fishingStats.allfish)
        {
            hasCaught.Add(fish, false);
        }


        for(int i = FishingStats.rarityTiers - 1; i > -1; i--)
        {
            GameObject newRarityBox = (Instantiate(fishingStats.rarityTierUIObject, container.transform));
            foreach(var fish in fishingStats.allfish)
            {
                if(fish.rarity == i)
                {
                    FishUI fishUI = Instantiate(fishingStats.fishUIObject, newRarityBox.transform).GetComponent<FishUI>();
                    fishUI.fish = fish;

                    if(hasCaught[fish])
                    {
                        fishUI.sprite = fish.sprite;
                    }
                    else
                    {
                        fishUI.sprite = fishingStats.uncaughtSprite;
                    }
                }
            }

            rarityTierBoxes.Add(newRarityBox);
        }
    }

    public void UpdateInventory()
    {
        foreach(var rarityBox in rarityTierBoxes)
        {
            foreach (Transform fishUIObject in rarityBox.transform)
            {
                FishUI fishUI = fishUIObject.GetComponent<FishUI>();
                if (hasCaught[fishUI.fish])
                {
                    fishUI.sprite = fishUI.fish.sprite;
                }
            }
        }
    }

    void OpenInventory()
    {
        canvasGroup.alpha = 1;
    }

    void CloseInventory()
    {
        canvasGroup.alpha = 0;
    }
}
