using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    [Header("Slots in the left panel")]
    [SerializeField]
    private List<Slot> slots;
    
    public ListViewController inventoryGrid;

    public Inventory inventory;
    public Equipment equipment;
    public ResourceContainer resources;

    public DropHandler inventoryDrop;
    public DropHandler sellDrop;
    public DropHandler deconstructDrop;

    private void Start()
    {
        UpdateView();
    }

    public void UpdateView()
    {
        inventoryGrid.InitView(inventory.records.Cast<object>().ToList());

        foreach (var item in equipment.equipedItems)
        {
            var slot = slots.Find(s => s.types.Contains(item.type));
            if (slot is not null)
                slot.slot.FillItem(inventory.GetRecord(item));
        }
    }

    public void OnInventoryDrop(GameObject gameObject)
    {
        var record = gameObject
            .GetComponent<InventoryListItem>().record;

        if (!record.isEquipped)
            equipment.Equip(record);
        else
            equipment.Unequip(record);

        UpdateView();
    }

    public void OnSellDrop(GameObject gameObject)
    {
        var record = gameObject
            .GetComponent<InventoryListItem>().record;
        inventory.Sell(record.recordGuid);

        UpdateView();
    }

    public void OnDeconstructDrop(GameObject gameObject)
    {
        var record = gameObject
            .GetComponent<InventoryListItem>().record;
        inventory.Deconstruct(record.recordGuid);

        UpdateView();
    }

    [Serializable]
    private class Slot
    {
        public InventoryListItem slot;
        public List<ItemType> types;
    } 
}