using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] FishingStats fishingStats;

    Dictionary<DiscoverableObject, bool> hasDiscovered = new();

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject lakeUIContainer;

    [SerializeField] FishInfoUI fishInfoUI;

    List<LakeInfoUI> lakeBoxes = new();

    ClickUI selectedBox = null;

    private void OnDestroy()
    {
        PlayerMovement.OnInventoryOpen -= OpenInventory;
        PlayerMovement.OnInventoryClose -= CloseInventory;
        LakeInfoUI.OnClickUI -= SetSelectedBox;
    }

    private void Awake()
    {
        PlayerMovement.OnInventoryOpen += OpenInventory;
        PlayerMovement.OnInventoryClose += CloseInventory;
        LakeInfoUI.OnClickUI += SetSelectedBox;
        
        InitializeInventory();
    }

    void InitializeInventory()
    {
        foreach (var table in fishingStats.allFishTables)
        {
            LakeInfoUI newLakeBox = (Instantiate(fishingStats.rarityTierUIObject, lakeUIContainer.transform).GetComponent<LakeInfoUI>());

            hasDiscovered.TryAdd(table, false);

            foreach(var pair in newLakeBox.Initialize(table.fishes, fishingStats, table))
            {
                hasDiscovered.TryAdd(pair.Key, pair.Value);
            }

            lakeBoxes.Add(newLakeBox);
        }
    }

    public void UpdateInventory(FishObject fish)
    {
        hasDiscovered.Remove(fish);
        hasDiscovered.Add(fish, true);

        foreach (var box in lakeBoxes)
        {
            bool discoveredLake = true;
            foreach (FishUI fishUI in box.fishUIs)
            {
                if (hasDiscovered[fishUI.fish])
                {
                    fishUI.Discover();
                }
                else
                {
                    discoveredLake = false;
                }
            }
            if(discoveredLake)
            {
                hasDiscovered.Remove(box.lake);
                hasDiscovered.TryAdd(box.lake, true);

            }

            if(hasDiscovered[box.lake])
            {
                box.Discover();
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

    void SetSelectedBox(ClickUI clicked)
    {
        selectedBox = clicked;

        FishInfo info = FishInfo.UndiscoveredInfo;

        if (hasDiscovered[clicked.discoverableObject])
        {
            info = clicked.GetClicked();
        }

        fishInfoUI.header = info.header;
        fishInfoUI.body = info.body;
    }
}
