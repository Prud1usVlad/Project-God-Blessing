using Assets.Scripts.Helpers;
using Assets.Scripts.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MainMenu.CharacterCreation
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CharacterStory", fileName = "Story")]
    public class CharacterStoryCard : SerializableScriptableObject
    {
        public string characterName;
        public Sprite avatar;
        public List<Stat> defaultStats = new() 
        { 
            new Stat { name = StatName.Sanity, baseValue = 0},
            new Stat { name = StatName.Agility, baseValue = 0},
            new Stat { name = StatName.Strength, baseValue = 0},
            new Stat { name = StatName.Charism, baseValue = 0},
            new Stat { name = StatName.Luck, baseValue = 0},
        };

        [TextArea]
        public string story;
    }
}
