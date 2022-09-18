using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FishUI : HoverUI
{
    Image image;


    public FishObject fish;
    Sprite _sprite;
    public Sprite sprite { get { return _sprite; } set { SetSprite(value); } }

    private void Awake()
    {
        image = GetComponent<Image>();

        hover.headerText = "???";
        hover.bodyText = "Catch this fish to learn more!";
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

   

    
}
