using Assets.Scripts.SkillSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/SkillSystem", fileName = "SkillSystem")]
public class SkillSystem : ScriptableObject
{
    public List<SkillRegistry> skillRegistries;
    public ConnectionsContainer connections;

    public void LearnSkill(NationName nation, string guid)
    {
        var connection = connections.GetConnection(nation);
        var skill = skillRegistries
            .Find(r => r.nation == nation)
            .FindByGuid(guid);

        if (connection.CanLearn(skill) && !skill.isLearnd) 
        {
            OnSkillLearn(skill.type);
            skill.isLearnd = true;
            connection.UseResearchPoint(skill);
        }
    }

    private void OnSkillLearn(SkillType type) 
    {
        switch(type)
        {
            case SkillType.Secret:

                break;
            case SkillType.Active:

                break;
            case SkillType.Building:

                break;
            case SkillType.Value:

                break;
            case SkillType.Device:

                break;
        }
    }
}