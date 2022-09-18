using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected HoverInfo hover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OpenHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CloseHover();
    }

    public void OpenHover()
    {
        hover.gameObject.SetActive(true);
    }

    public void CloseHover()
    {
        hover.gameObject.SetActive(false);
    }
}
