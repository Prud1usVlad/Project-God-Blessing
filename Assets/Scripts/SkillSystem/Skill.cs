using Assets.Scripts.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SkillSystem
{
    public abstract class Skill : SerializableScriptableObject
    {
        public string skillName;
        public SkillType type;
        public NationName nation;
        [TextArea]
        public string description;
        [Range(1, 3)]
        public int level;
        public int pointsRequired = 1;
        public bool isLearnd = false;

        public List<Skill> required;
        public List<Building> buildingUpgrades;
    }
}
