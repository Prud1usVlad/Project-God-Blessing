using Assets.Scripts.EquipmentSystem;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Stats;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentSystem/Equipment", fileName = "Equipment")]
public class Equipment : ScriptableObject
{
    public GameProgress gameProgress;
    public EquipmentItemRegistry itemsRegistry;
    public Dictionary<SlothType, InventoryRecord> equipedItems;

    public InventoryRecord GetItemByName(string name)
    {
        return equipedItems.Values.First(i => i.item.itemName == name);
    }

    public InventoryRecord GetItemBySlothType(SlothType type)
    {
        if (equipedItems.ContainsKey(type))
            return equipedItems[type];
        else
            return null;
    }

    public InventoryRecord GetItemByGuid(string guid)
    {
        return equipedItems.Values.First(i => i.itemGuid == guid);
    }

    public InventoryRecord GetItemByType(ItemType type)
    {
        return equipedItems.Values.First(i => i.item.type == type);
    }

    public InventoryRecord GetEquipedAnalogue(EquipmentItem item)
    {
        return GetItemBySlothType(item.slothType);
    }

    public Dictionary<StatName, List<StatModifier>> GetModifiers()
    {
        var res = new Dictionary<StatName, List<StatModifier>>();

        foreach (var i in equipedItems.Values)
        {
            foreach (var m in i.item.modifiers.statModifiers)
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
            equipedItems.Add(item.slothType, record);
        else
        {
            Unequip(equipedItems[item.slothType]);
            equipedItems[item.slothType] = record;
        }

        // Check if we need to take off some additional equipment
        if (item.slothType == SlothType.Weapon
            && equipedItems.ContainsKey(SlothType.Accesory))
        {
            var accesory = equipedItems[SlothType.Accesory];
            if (accesory.item.complementary != item.type)
                Unequip(accesory);
        }
        else if (item.slothType == SlothType.Accesory 
            && equipedItems.ContainsKey(SlothType.Weapon))
        {
            var weapon = equipedItems[SlothType.Weapon];
            if (weapon.item.complementary != item.type) 
                Unequip(weapon);
        }
        
        record.isEquipped = true;
        gameProgress.globalModifiers.AddModifiers(item.modifiers);

        return true;
    }

    public void Unequip(InventoryRecord record)
    {
        if (record is not null 
            && equipedItems.Values.Contains(record))
        {
            record.isEquipped = false;
            equipedItems.Remove(record.item.slothType);
            gameProgress.globalModifiers.RemoveModifiers(record.item.modifiers);
        }
    }

    private void Unequip(EquipmentItem item)
    {
        Unequip(GetItemByGuid(item.Guid));
    }

    private void OnEnable()
    {
        equipedItems = new();
    }
}
