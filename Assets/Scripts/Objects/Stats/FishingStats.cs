using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Fishing Stats")]
public class FishingStats : ScriptableObject
{
    public static readonly int rarityTiers = 5;
    public static string[] rarityNames = {"Common", "Uncommon", "Rare", "Epic", "Legendary" };

    [Header("Fishing")]


    [SerializeField] List<Sprite> _rarityPings = new(new Sprite[rarityTiers]);
    [SerializeField] List<float> _rarityTimers = new(new float[rarityTiers]);
    [SerializeField] GameObject _pingObject;
    [SerializeField] GameObject _notificationObject;
    [SerializeField] float _notificationTimer;
    [SerializeField] float _reelPower;
    [SerializeField] List<float> _rarityShake = new(new float[rarityTiers]);
    [SerializeField] List<FishTable> _allFishTables = new();
    [SerializeField] GameObject _rarityTierUIObject;
    [SerializeField] GameObject _fishUIObject;
    [SerializeField] Sprite _uncaughtSprite;
    [SerializeField] float _fishPullTime; //time between pulls
    public List<Sprite> rarityPings => _rarityPings;
    public List<float> rarityTimers => _rarityTimers;
    public GameObject pingObject => _pingObject;
    public GameObject notificationObject => _notificationObject;
    public float notificationTimer => _notificationTimer;
    public float reelPower => _reelPower;
    public List<float> rarityShake => _rarityShake;
    public List<FishTable> allFishTables => _allFishTables;
    public GameObject rarityTierUIObject => _rarityTierUIObject;
    public GameObject fishUIObject => _fishUIObject;
    public Sprite uncaughtSprite => _uncaughtSprite;
    public float fishPullTimer => _fishPullTime;
}
