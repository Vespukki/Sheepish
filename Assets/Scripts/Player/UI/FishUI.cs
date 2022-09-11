using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishUI : MonoBehaviour
{
    Image image;

    public FishObject fish;
    Sprite _sprite;
    public Sprite sprite { get { return _sprite; } set { SetSprite(value); } }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void SetSprite(Sprite value)
    {
        image.sprite = value;
        _sprite = value;
    }
}
