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

    List<LakeInfoUI> lakeBoxes = new();

    private void OnDestroy()
    {
        PlayerMovement.OnInventoryOpen -= OpenInventory;
        PlayerMovement.OnInventoryClose -= CloseInventory;
    }

    private void Awake()
    {
        PlayerMovement.OnInventoryOpen += OpenInventory;
        PlayerMovement.OnInventoryClose += CloseInventory;

        InitializeInventory();
    }

    void InitializeInventory()
    {
        foreach (var table in fishingStats.allFishTables)
        {
            LakeInfoUI newLakeBox = (Instantiate(fishingStats.rarityTierUIObject, container.transform).GetComponent<LakeInfoUI>());
            foreach(var pair in newLakeBox.Initialize(table.fishes, fishingStats, table.tableName))
            {
                hasCaught.TryAdd(pair.Key, pair.Value);
            }

            lakeBoxes.Add(newLakeBox);
        }
    }

    public void UpdateInventory()
    {
        foreach(var box in lakeBoxes)
        {
            foreach (FishUI fishUI in box.fishUIs)
            {
                if (hasCaught[fishUI.fish])
                {
                    fishUI.Discover();
                }
            }
        }
    }

    void OpenInventory()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void CloseInventory()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
