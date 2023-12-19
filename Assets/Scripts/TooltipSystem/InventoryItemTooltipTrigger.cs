using System;
using UnityEngine.EventSystems;
using UnityEngine;
using Assets.Scripts.EquipmentSystem;
using System.Collections;

public class InventoryItemTooltipTrigger :
    MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryRecord item;
    private InventoryRecord compare;
    public float delay = 0.5f;
    public bool showCompare = true;

    public void Init(InventoryRecord record, InventoryRecord compareRecord = null)
    {
        this.item = record;
        this.compare = compareRecord;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(ShowRoutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        TooltipSystem.Hide();
    }

    private IEnumerator ShowRoutine()
    {
        yield return new WaitForSeconds(delay);
        TooltipSystem.Show(item, showCompare ? compare : null);
    }
}
