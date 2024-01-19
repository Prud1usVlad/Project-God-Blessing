using Assets.Scripts.StatSystem;
using UnityEngine;

namespace Assets.Scripts.SkillSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skills/Value", fileName = "Value")]
    public class ValueSkill : Skill
    {
        public ConditionalModifier conditional;
    }
}
