using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LakeInfoUI : ClickUI
{
    [SerializeField] TextMeshProUGUI nameTextmesh;
    [SerializeField] Transform fishUIHolder;
    FishTable _lake;
    public FishTable lake { get { return _lake; } set { SetLake(value); } }

    public List<FishUI> fishUIs = new();
    void SetLake(FishTable value)
    {
        _lake = value;
        discoverableObject = value;

        nameTextmesh.SetText(FishInfo.UndiscoveredInfo.header);
    }

    public List<KeyValuePair<FishObject, bool>> Initialize(List<FishTableEntry> entries, FishingStats stats, FishTable table)
    {
        lake = table;

        List<KeyValuePair<FishObject, bool>> returnList = new();

        foreach (var entry in entries)
        {
            FishUI fishUI = Instantiate(stats.fishUIObject, fishUIHolder).GetComponentInChildren<FishUI>();
            fishUI.fish = entry.fish;
            fishUI.sprite = stats.uncaughtSprite;

            returnList.Add( new(entry.fish, false));
            fishUIs.Add(fishUI);

        }

        return returnList;
    }

    public override void Discover()
    {
        nameTextmesh.SetText(lake.discoverName);
    }
}
