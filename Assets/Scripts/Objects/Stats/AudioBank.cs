using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Audio Bank")]
public class AudioBank : ScriptableObject
{
    [Header("Fishing")]
    [SerializeField] List<AudioClip> _fishPing = new(new AudioClip[FishingStats.rarityTiers]);
    [SerializeField] AudioClip _upgrade;
    public List<AudioClip> fishPing => _fishPing;
    public AudioClip upgrade => _upgrade;
}
