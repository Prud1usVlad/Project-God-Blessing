using Assets.Scripts.SkillSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Assets.Scripts.Models;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/SkillSystem", fileName = "SkillSystem")]
public class SkillSystem : ScriptableObject
{
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
        }
    }

    private void OnSkillLearn(SkillType type, Skill skill) 
    {
        switch(type)
        {
            case SkillType.Secret:

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
    }

    private void OnBuildingLearn(Skill skill)
    {
        var building = (skill as BuildingSkill).building;
        var rb = gameProgress.buildingResearch
            .Find(b => b.guid == building.Guid);

        if (rb is not null)
            rb.isAvaliable = true;
        else
            gameProgress.buildingResearch.Add(new ItemAvaliability
            {
                guid = building.Guid,
                isAvaliable = true
            });
    }
}