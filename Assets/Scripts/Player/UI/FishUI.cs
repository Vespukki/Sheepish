using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FishUI : ClickUI
{
    Image image;


    public FishObject fish;
    Sprite _sprite;
    public Sprite sprite { get { return _sprite; } set { SetSprite(value); } }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Discover()
    {
        sprite = fish.sprite;

    }

    void SetSprite(Sprite value)
    {
        image.sprite = value;
        _sprite = value;
    }

    public override FishInfo GetClicked()
    {
        return new FishInfo(fish.fishName, fish.description);
    }
}
