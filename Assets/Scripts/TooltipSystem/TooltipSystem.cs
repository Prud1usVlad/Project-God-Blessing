using Assets.Scripts.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem instance;

    public Tooltip tooltip;
    public GameObject inventoryTooltipWrapper;
    public InventoryItemTooltip itemTooltip;
    public InventoryItemTooltip compareTooltip;

    public void Awake()
    {
        instance = this;
    }

    public static void Show(InventoryRecord record, InventoryRecord compareRecord = null)
    {
        if (record is null)
            return;

        instance.itemTooltip.InitView(record);
        if (compareRecord != null) 
        {
            instance.compareTooltip.InitView(compareRecord);
        }

        instance.inventoryTooltipWrapper.SetActive(true);
        instance.compareTooltip.gameObject.SetActive(compareRecord != null);
    }

    public static void Show(string content, string header = "")
    {
        instance.tooltip.InitView(content, header);
        instance.tooltip.gameObject.SetActive(true);
    }

    public static void Hide() 
    {
        instance.tooltip.gameObject.SetActive(false);
        instance.inventoryTooltipWrapper.SetActive(false);
    }
}
