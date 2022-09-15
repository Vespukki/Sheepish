using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LakeInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTextmesh;
    [SerializeField] Transform fishUIHolder;
    string _lakeName;
    public string lakeName { get { return _lakeName; } set { SetName(value); } }

    public List<FishUI> fishUIs = new();
    void SetName(string value)
    {
        _lakeName = value;
        nameTextmesh.SetText(_lakeName);
    }

    public List<KeyValuePair<FishObject, bool>> Initialize(List<FishTableEntry> entries, FishingStats stats, string tableName)
    {
        lakeName = tableName;

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
