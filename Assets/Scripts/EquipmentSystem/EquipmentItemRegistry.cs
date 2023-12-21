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

    public List<EquipmentItem> GetByType(ItemType type)
    {
        return _descriptors
            .Where(i => i.type == type).ToList();
    }

    public List<EquipmentItem> GetBySlothType(SlothType sloth)
    {
        return _descriptors.Where(i => i.slothType == sloth).ToList();
    }

    public EquipmentItem GetByName(string name)
    {
        return _descriptors.Find(i => i.name == name);
    }

    public List<EquipmentItem> GetAll()
    {
        return _descriptors;
    }
}
