using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fish Table")]
public class FishTable : ScriptableObject
{
    [SerializeField] List<FishTableEntry> _fishes = new List<FishTableEntry>();
    [SerializeField] string _tableName;
    [SerializeField] float _minTime;
    [SerializeField] float _maxTime;
    [TextArea] [SerializeField] string _description;
    public List<FishTableEntry> fishes => _fishes;
    public string tableName => _tableName;
    public float minTime => _minTime;
    public float maxTime => _maxTime;
     public string description => _description;
}

[System.Serializable]
public class FishTableEntry
{
    public FishObject fish;

    public int chancePart;
}