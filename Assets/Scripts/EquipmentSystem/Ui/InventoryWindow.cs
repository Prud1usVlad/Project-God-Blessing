using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryWindow : DialogueBox
{
    public GameObject skillManagementWindowPrefab;

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

    public override bool InitDialogue()
    {
        var inited = base.InitDialogue();

        if (inited)
        {
            UpdateView();
        }

        return inited;
    }

    public void UpdateView()
    {
        inventoryGrid.InitView(inventory.records.Cast<object>().ToList());

        slots.ForEach(s => s.slot.FillItem(null));

        foreach (var record in equipment.records)
        {
            var item = equipment.GetItemByGuid(record.itemGuid);
            var slot = slots.Find(s => s.types.Contains(item.type));
            if (slot is not null)
                slot.slot.FillItem(inventory.GetRecord(record.recordGuid));
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

    public void OnClose()
    {
        modalManager.DialogueClose();
    }

    public void OnManageSkills()
    {
        Instantiate(skillManagementWindowPrefab, transform);
    }

    [Serializable]
    private class Slot
    {
        public InventoryListItem slot;
        public List<ItemType> types;
    } 
}