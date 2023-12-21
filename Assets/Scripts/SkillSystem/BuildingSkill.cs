using UnityEngine;

namespace Assets.Scripts.SkillSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skills/Building", fileName = "Building")]
    public class BuildingSkill : Skill
    {
        public Building building;
    }
}
