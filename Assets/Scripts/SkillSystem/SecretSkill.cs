using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SkillSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skills/Secret", fileName = "Secret")]
    public class SecretSkill : Skill
    {
        public bool isPublished = false;
        [Tooltip("Message that will be shown on secret publishing")]
        public string publishMessage;

        //public List<StatMod> 
    }
}
