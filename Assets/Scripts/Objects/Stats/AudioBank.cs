using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Audio Bank")]
public class AudioBank : ScriptableObject
{
    [Header("Fishing")]
    [SerializeField] List<AudioClip> _fishPing = new(new AudioClip[FishingStats.rarityTiers]);
    public List<AudioClip> fishPing => _fishPing;
}
