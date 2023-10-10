﻿using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.SkillSystem
{
    public abstract class Skill : SerializableScriptableObject
    {
        public string skillName;
        public SkillType type;
        public string description;
        [Range(1, 3)]
        public int level;
        public int pointsRequired = 1;
        public bool isLearnd = false;
    }
}
