using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fish Table")]
public class FishTable : ScriptableObject
{
    [SerializeField] List<FishTableEntry> _fishes = new List<FishTableEntry>();
    [SerializeField] float _minTime;
    [SerializeField] float _maxTime;
    public List<FishTableEntry> fishes => _fishes;
    public float minTime => _minTime;
    public float maxTime => _maxTime;
}

[System.Serializable]
public class FishTableEntry
{
    public FishObject fish;

    public int chancePart;
}