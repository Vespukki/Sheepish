using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Fishing Stats")]
public class FishingStats : ScriptableObject
{
    public static readonly int rarityTiers = 5;

    [Header("Fishing")]


    [SerializeField] List<Sprite> _rarityPings = new(new Sprite[rarityTiers]);
    [SerializeField] List<float> _rarityTimers = new(new float[rarityTiers]);
    [SerializeField] GameObject _pingObject;
    [SerializeField] GameObject _notificationObject;
    [SerializeField] float _notificationTimer;
    [SerializeField] float _reelPower;
    [SerializeField] List<float> _rarityShake = new(new float[rarityTiers]);
    [SerializeField] List<FishObject> _allFish = new(new FishObject[5]);
    [SerializeField] GameObject _rarityTierUIObject;
    [SerializeField] GameObject _fishUIObject;
    [SerializeField] Sprite _uncaughtSprite;
    public List<Sprite> rarityPings => _rarityPings;
    public List<float> rarityTimers => _rarityTimers;
    public GameObject pingObject => _pingObject;
    public GameObject notificationObject => _notificationObject;
    public float notificationTimer => _notificationTimer;
    public float reelPower => _reelPower;
    public List<float> rarityShake => _rarityShake;
    public List<FishObject> allfish => _allFish;
    public GameObject rarityTierUIObject => _rarityTierUIObject;
    public GameObject fishUIObject => _fishUIObject;
    public Sprite uncaughtSprite => _uncaughtSprite;
}
