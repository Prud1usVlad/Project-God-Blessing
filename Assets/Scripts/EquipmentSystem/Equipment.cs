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
    //public List<EquipmentItem> equipedItems;
    public Dictionary<SlothType, EquipmentItem> equipedItems;
    public List<InventoryRecord> records;

    public EquipmentItem GetItemByName(string name)
    {
        return equipedItems.Values.First(i => i.itemName == name);
    }

    public EquipmentItem GetItemBySlothType(SlothType type)
    {
        if (equipedItems.ContainsKey(type))
            return equipedItems[type];
        else
            return null;
    }

    public EquipmentItem GetItemByGuid(string guid)
    {
        return equipedItems.Values.First(i => i.Guid == guid);
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
        return equipedItems.Values.First(i => i.type == type);
    }

    public EquipmentItem GetEquipedAnalogue(EquipmentItem item)
    {
        return GetItemBySlothType(item.slothType);
    }

    public Dictionary<StatName, List<StatModifier>> GetModifiers()
    {
        var res = new Dictionary<StatName, List<StatModifier>>();

        foreach (var item in equipedItems.Values)
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

        if (!equipedItems.ContainsKey(item.slothType))
            equipedItems.Add(item.slothType, item);
        else
        {
            Unequip(equipedItems[item.slothType]);
            equipedItems[item.slothType] = item;
        }

        // Check if we need to take off some additional equipment
        if (item.slothType == SlothType.Weapon
            && equipedItems.ContainsKey(SlothType.Accesory))
        {
            var accesory = equipedItems[SlothType.Accesory];
            if (accesory.complementary != item.type)
                Unequip(accesory);
        }
        else if (item.slothType != SlothType.Accesory 
            && equipedItems.ContainsKey(SlothType.Weapon))
        {
            var weapon = equipedItems[SlothType.Weapon];
            if (weapon.complementary != item.type) 
                Unequip(weapon);
        }
        
        record.isEquipped = true;
        records.Add(record);

        return true;
    }

    public void Unequip(InventoryRecord record)
    {
        if (record is not null 
            && records.Contains(record))
        {
            record.isEquipped = false;
            records.Remove(record);
            equipedItems.Remove(equipedItems.Values
                .First(i => i.Guid == record.itemGuid).slothType);
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
