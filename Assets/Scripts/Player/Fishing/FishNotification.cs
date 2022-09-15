using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishNotification : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI textmesh;


    public void FormatNotification(Fish fish)
    {
        textmesh.SetText(string.Format("{1} star catch! you caught a {0}!", fish.info.fishName, fish.info.rarity + 1));
        image.sprite = fish.info.sprite;
    }
}
