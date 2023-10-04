using Assets.Scripts.Helpers;
using Assets.Scripts.Registries;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Registries/FameLevel", fileName = "BuildingRegistry")]
public class FameLevelRegistry : Registry<FameLevel>, IListViewExtendedRegistry
{
    public int count => _descriptors.Count;
    public List<FameLevel> levels => _descriptors;

    public FameLevel GetByIndex(int index)
    {
        return _descriptors[index];
    }
    
    public object Find(string guid)
    {
        return FindByGuid(guid);
    }

    public void ForEach(Action<object> action)
    {
        foreach (var desc in _descriptors)
        {
            action(desc);
        }
    }

    private void OnEnable()
    {
        _descriptors.Sort((a, b) => a.points - b.points);
    }
} 