using Assets.Scripts.Helpers;
using Assets.Scripts.Registries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Registries/Buildings", fileName = "BuildingRegistry")]
public class BuildingRegistry : Registry<Building>, IListViewExtendedRegistry
{
    public IEnumerable<Building> Buildings { get { return _descriptors; } }

    public object Find(string guid)
    {
        return FindByGuid(guid);
    }

    public Building FindByName(string name)
    {
        foreach (var desc in _descriptors)
        {
            if (desc.buildingName == name)
            {
                return desc;
            }
        }

        return null;
    }

    public void ForEach(Action<object> action)
    {
        foreach (var desc in _descriptors)
        {
            action(desc);
        }
    }
}
