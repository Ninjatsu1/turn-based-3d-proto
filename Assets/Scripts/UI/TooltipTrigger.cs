using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Header = "Header text";
    public string Content = "Content text. Epic";

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.DisplayTooltip(Header, Content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.HideTooltip();
    }
}

