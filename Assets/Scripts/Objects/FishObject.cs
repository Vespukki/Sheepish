using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fish")]
public class FishObject : DiscoverableObject
{
    [SerializeField] Sprite _sprite;
    [Range(0, 4)][SerializeField] int _rarity;
    public Sprite sprite => _sprite;
    public int rarity => _rarity;

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