using Assets.Scripts.EquipmentSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Registries/EquipmentItem", fileName = "EquipmentItem")]
public class EquipmentItemRegistry : Registry<EquipmentItem>
{
    public List<EquipmentItem> GetByNation(NationName nation)
    {
        return _descriptors
            .Where(n => n.nation == nation).ToList();
    }

    public List<EquipmentItem> GetByLevel(int level, bool allowLower = false)
    {
        if (allowLower)
            return _descriptors.Where(i => i.level <= level).ToList();
        else
            return _descriptors.Where(i => i.level == level).ToList();
    }

    public List<EquipmentItem> GetByLevel(int min, int max)
    {
        return _descriptors.Where(i 
            => i.level <= max && i.level >= min).ToList();
    }

    public List<EquipmentItem> GetByType(ItemType type)
    {
        return _descriptors
            .Where(i => i.type == type).ToList();
    }

    public EquipmentItem GetByName(string name)
    {
        return _descriptors.Find(i => i.name == name);
    }
}
