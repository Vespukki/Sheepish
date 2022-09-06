using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Player/Fishing Stats")]
public class FishingStats : ScriptableObject
{
    public static readonly int rarityTiers = 5;

    [Header("Fishing")]


    [SerializeField] public List<Sprite> _rarityPings = new(new Sprite[rarityTiers]);
    [SerializeField] List<float> _rarityTimers = new( new float[rarityTiers]);
    [SerializeField] GameObject _pingObject;
    [SerializeField] GameObject _notificationObject;
    [SerializeField] float _notificationTimer;

    public List<Sprite> rarityPings => _rarityPings;
    public List<float> rarityTimers => _rarityTimers;
    public GameObject pingObject => _pingObject;
    public GameObject notificationObject => _notificationObject;
    public float notificationTimer => _notificationTimer;
}
