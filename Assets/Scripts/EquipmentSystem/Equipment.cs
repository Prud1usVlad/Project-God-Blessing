using Assets.Scripts.EquipmentSystem;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Stats;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentSystem/Equipment", fileName = "Equipment")]
public class Equipment : ScriptableObject
{
    public EquipmentItemRegistry itemsRegistry;
    public List<EquipmentItem> equipedItems;
    public List<InventoryRecord> records;

    public EquipmentItem GetItemByName(string name)
    {
        return equipedItems.Find(i => i.itemName == name);
    }

    public EquipmentItem GetItemByGuid(string guid)
    {
        return equipedItems.Find(i => i.Guid == guid);
    }

    public EquipmentItem GetItemByRecord(InventoryRecord record)
    {
        if (record.isEquipped) 
            return GetItemByGuid(record.itemGuid);
        else 
            return null;
    }

    public EquipmentItem GetItemByType(ItemType type)
    {
        return equipedItems.Find(i => i.type == type);
    }

    public EquipmentItem GetEquipedAnalogue(EquipmentItem item)
    {
        var equiped = GetItemByType(item.type);

        if (equiped is null)
        {
            foreach(var ex in item.type.GetAnalogues())
            {
                equiped = GetItemByType(ex);
                if (equiped is not null)
                    break;
            }
        }

        return equiped;
    }

    public Dictionary<StatName, List<StatModifier>> GetModifiers()
    {
        var res = new Dictionary<StatName, List<StatModifier>>();

        foreach (var item in equipedItems)
        {
            foreach (var m in item.modifiers)
            {
                if (res.ContainsKey(m.stat))
                    res[m.stat].Add(m.modifier);
                else
                    res.Add(m.stat, new() { m.modifier });
            }
        }

        return res;
    }

    public bool Equip(InventoryRecord record, bool checkIsEquiped = true)
    {
        if (record.isEquipped && checkIsEquiped)
            return false;

        var item = itemsRegistry.FindByGuid(record.itemGuid);
        var excluded = item.type.GetExcluded();
        var toUnequip = new List<EquipmentItem>();

        foreach (var eItem in equipedItems)
        {
            if (eItem.type == item.type ||
                excluded.Contains(eItem.type))
            {
                    toUnequip.Add(eItem);
            }
        }

        toUnequip.ForEach(Unequip);

        record.isEquipped = true;
        records.Add(record);
        equipedItems.Add(item);

        return true;
    }

    public void Unequip(InventoryRecord record)
    {
        if (record is not null 
            && records.Contains(record))
        {
            record.isEquipped = false;
            records.Remove(record);
            equipedItems.Remove(equipedItems
                .Find(i => i.Guid == record.itemGuid));
        }
    }

    private void Unequip(EquipmentItem item)
    {
        Unequip(records.Find(r => r.itemGuid == item.Guid));
    }

    private void OnEnable()
    {
        equipedItems = new();
        records = new();
    }
}
