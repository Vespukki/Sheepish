using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fish")]
public class FishObject : ScriptableObject
{
    [SerializeField] string _fishName;
    [SerializeField] Sprite _sprite;
    [Range(0, 4)][SerializeField] int _rarity;
    [TextArea][SerializeField] string _description;
    public string fishName => _fishName;
    public Sprite sprite => _sprite;
    public int rarity => _rarity;
    public string description => _description;
}

[System.Serializable]
public class Fish
{
    public FishObject info;

    public Fish(FishObject _info)
    {
        info = _info;
    }
}