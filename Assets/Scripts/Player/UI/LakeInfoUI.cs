using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LakeInfoUI : ClickUI
{
    [SerializeField] TextMeshProUGUI nameTextmesh;
    [SerializeField] Transform fishUIHolder;
    FishTable _lake;
    public FishTable lake { get { return _lake; } set { SetName(value); } }

    public List<FishUI> fishUIs = new();
    void SetName(FishTable value)
    {
        _lake = value;
        nameTextmesh.SetText(value.tableName);
    }

    public override FishInfo GetClicked()
    {
        return new FishInfo(lake.tableName, lake.description);
    }

    public List<KeyValuePair<FishObject, bool>> Initialize(List<FishTableEntry> entries, FishingStats stats, FishTable table)
    {
        lake = table;

        List<KeyValuePair<FishObject, bool>> returnList = new();

        foreach (var entry in entries)
        {
            FishUI fishUI = Instantiate(stats.fishUIObject, fishUIHolder).GetComponent<FishUI>();
            fishUI.fish = entry.fish;
            fishUI.sprite = stats.uncaughtSprite;

            returnList.Add( new(entry.fish, false));
            fishUIs.Add(fishUI);

        }

        return returnList;
    }
}
