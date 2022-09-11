using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fish")]
public class FishObject : ScriptableObject
{
    [SerializeField] string _fishName;
    [SerializeField] Sprite _sprite;
    [Range(0, 4)][SerializeField] int _rarity;
    [SerializeField] float _minWeight;
    [SerializeField] float _maxWeight;
    public string fishName => _fishName;
    public Sprite sprite => _sprite;
    public int rarity => _rarity;
    public float minWeight => _minWeight;
    public float maxWeight => _maxWeight;

}

[System.Serializable]
public class Fish
{
    public FishObject info;
    public float weight;

    public Fish(FishObject _info)
    {
        info = _info;
        weight = GetWeight();
    }
    public Fish(FishObject _info, float _weight)
    {
        info = _info;
        weight = _weight;
    }

    float GetWeight()
    {
        return Random.Range(info.minWeight, info.maxWeight);
        //TODO: make distribution normal instead of random
    }
}