using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SkillSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skills/Secret", fileName = "Secret")]
    public class SecretSkill : Skill
    {
        public GameProgress gameProgress;
        public bool isPublished = false;
        [Tooltip("Message that will be shown on secret publishing")]
        [TextArea]
        public string publishMessage;

        public ModifiersContainer secretModifiers;
        public ModifiersContainer publishedModifiers;
    
        public void OnLearn()
        {
            gameProgress.globalModifiers.AddModifiers(secretModifiers);
        }

        public void OnPublish()
        {
            gameProgress.globalModifiers.RemoveModifiers(secretModifiers);
            gameProgress.globalModifiers.AddModifiers(publishedModifiers);
            isPublished = true;
        }
    }
}
