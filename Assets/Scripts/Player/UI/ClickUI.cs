using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ClickUI : MonoBehaviour, IPointerClickHandler
{
    public delegate void UICLickEvent(ClickUI clickUI);
    public static event UICLickEvent OnClickUI;


    public void OnPointerClick(PointerEventData eventData)
    {
        if(OnClickUI != null)
        {
            OnClickUI(this);
        }
    }

    public abstract FishInfo GetClicked();
}
