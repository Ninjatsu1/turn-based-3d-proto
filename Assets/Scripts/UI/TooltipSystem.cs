using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;

    public Tooltip Tooltip;

    public void Awake()
    {
        current = this;
    }

    public static void DisplayTooltip(string header = "", string content = "")
    {
        current.Tooltip.SetTooltipText(header, content);
        current.Tooltip.gameObject.SetActive(true);
    }

    public static void HideTooltip()
    {
        current.Tooltip.gameObject.SetActive(false);
    }
}
