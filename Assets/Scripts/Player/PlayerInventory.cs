using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] FishingStats fishingStats;
    public Dictionary<FishObject, bool> hasCaught = new();

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
            foreach(var pair in newLakeBox.Initialize(table.fishes, fishingStats, table))
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

    void SetSelectedBox(ClickUI clicked)
    {
        selectedBox = clicked;

        FishInfo info = clicked.GetClicked();

        fishInfoUI.header = info.header;
        fishInfoUI.body = info.body;
    }
}
