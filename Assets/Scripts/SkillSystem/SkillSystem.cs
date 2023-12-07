using Assets.Scripts.SkillSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Assets.Scripts.Models;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/SkillSystem", fileName = "SkillSystem")]
public class SkillSystem : ScriptableObject
{
    public List<Skill> learnedSkills;
    public List<ActiveSkill> equipedActiveSkills;
    public ValueSkill equipedValueSkill;

    public List<SkillRegistry> skillRegistries;
    public ConnectionsContainer connections;
    public GameProgress gameProgress;

    public void LearnSkill(NationName nation, string guid)
    {
        var connection = connections.GetConnection(nation);
        var skill = skillRegistries
            .Find(r => r.nation == nation)
            .FindByGuid(guid);

        if (connection.CanLearn(skill) && !skill.isLearnd) 
        {
            OnSkillLearn(skill.type, skill);
            skill.isLearnd = true;
            connection.UseResearchPoint(skill);
            learnedSkills.Add(skill);
        }
    }

    public ActiveSkill GetActive(int index)
    {
        return equipedActiveSkills[index];
    }

    public ValueSkill GetValue()
    {
        return equipedValueSkill;
    }

    public void Equip(ActiveSkill skill, int index)
    {
        if (IsEquiped(skill))
            return;

        if (index < 3 && index >= 0) 
        {
            equipedActiveSkills[index] = skill;
        }
    }

    public void Equip(ValueSkill skill)
    {
        if (IsEquiped(skill))
            return;

        equipedValueSkill = skill;
    }

    public bool IsEquiped(Skill skill)
    {
        if (skill is not ActiveSkill && skill is not ValueSkill)
            return false;
        else
        {
            var res = skill.Equals(equipedValueSkill);

            if (res)
                return res;
            else
                return equipedActiveSkills.FirstOrDefault(s => skill.Equals(s)) is not null;
        }
    }

    private void OnSkillLearn(SkillType type, Skill skill) 
    {
        switch(type)
        {
            case SkillType.Secret:
                (skill as SecretSkill).OnLearn(); 
                break;
            case SkillType.Active:

                break;
            case SkillType.Building:
                OnBuildingLearn(skill);
                break;
            case SkillType.Value:

                break;
            case SkillType.Device:

                break;
        }

        if (skill.buildingUpgrades is not null 
            && skill.buildingUpgrades.Count > 0)
        {
            foreach(var upgrade in skill.buildingUpgrades) 
            {
                AddReserchedBuilding(upgrade);
            }
        }
    }

    private void OnBuildingLearn(Skill skill)
    {
        var building = (skill as BuildingSkill).building;
        if (building == null)
            return;
        AddReserchedBuilding(building);
    }

    private void AddReserchedBuilding(Building building)
    {
        var rb = gameProgress.buildingResearch?
                    .Find(b => b.guid == building.Guid);

        if (rb is not null)
            rb.isAvailable = true;
        else
            gameProgress.buildingResearch.Add(new ItemAvaliability
            {
                guid = building.Guid,
                isAvailable = true
            });
    }

    private void OnEnable()
    {
        equipedActiveSkills = new() { null, null, null };
    }
}