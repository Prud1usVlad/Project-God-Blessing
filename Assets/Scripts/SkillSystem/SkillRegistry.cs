using Assets.Scripts.Registries;
using Assets.Scripts.SkillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Registries/Skills", fileName = "SkillRegistry")]
public class SkillRegistry : Registry<Skill>, IListViewExtendedRegistry
{
    public NationName nation;

    public IEnumerable<Skill> skills => _descriptors;

    public Skill FindByName(string name)
    {
        return _descriptors.Find(s => s.skillName == name);
    }

    public object Find(string guid)
    {
        return FindByGuid(guid);
    }

    public void ForEach(Action<object> action)
    {
        foreach (var item in _descriptors) 
        {
            action(item);
        }
    }

    public IEnumerable<Skill> GetByType(SkillType type) 
    {
        return _descriptors.Where(s => s.type == type);
    }

    public IEnumerable<Skill> GetByLevel(int level, bool withLower = false) 
    {
        if (withLower)
            return _descriptors.Where(s => s.level <= level);
        else 
            return _descriptors.Where(s => s.level == level);
    }

    public IEnumerable<Skill> GetLerned(bool inverse = false)
    {
        if (inverse)
            return _descriptors.Where(s => !s.isLearnd);
        else
            return _descriptors.Where(s => s.isLearnd);
    }
}
