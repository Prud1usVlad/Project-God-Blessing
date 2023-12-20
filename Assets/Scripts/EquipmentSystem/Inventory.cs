using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentSystem/Inventory", fileName = "Inventory")]
public class Inventory : ScriptableObject
{
    public EquipmentItemRegistry itemRegistry;
    public ResourceContainer resourceContainer;
    public Equipment equipment;

    public List<InventoryRecord> records;

    public void Add(InventoryRecord record)
    {
        records.Add(record);
    }

    public void Sell(string recordGuid)
    {
        var record = records.Find(r => r.recordGuid == recordGuid);
        var item = itemRegistry.FindByGuid(record.itemGuid);

        if (record.isEquipped)
            equipment.Unequip(record);

        records.Remove(record);

        resourceContainer.Gain(item.sellPrice);
    }

    public void Deconstruct(string recordGuid)
    {
        var record = records.Find(r => r.recordGuid == recordGuid);
        var item = itemRegistry.FindByGuid(record.itemGuid);

        if (record.isEquipped)
            equipment.Unequip(record);

        records.Remove(record);

        resourceContainer.Gain(item.deconstructionPrice);
    }

    public InventoryRecord GetRecord(string guid)
    {
        return records.Find(r => r.recordGuid == guid);
    }

    private void OnEnable()
    {
        records = new();
    }
}
