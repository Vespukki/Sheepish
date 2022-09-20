using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FishUI : ClickUI
{
    Image image;

    FishObject _fish;
    public FishObject fish { get { return _fish; } set { _fish = value; discoverableObject = value; }}
    Sprite _sprite;
    public Sprite sprite { get { return _sprite; } set { SetSprite(value); } }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public override void Discover()
    {
        sprite = fish.sprite;
    }

    void SetSprite(Sprite value)
    {
        image.sprite = value;
        _sprite = value;
    }

    public override FishInfo GetUndiscoveredInfo()
    {
        return new("???????", "Catch this fish to learn more!");
    }
}
