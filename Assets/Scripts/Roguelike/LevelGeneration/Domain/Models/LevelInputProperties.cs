using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using UnityEngine;

namespace Assets.Scripts.Roguelike.LevelGeneration.Domain.Models
{
    [System.Serializable]
    public class LevelInputProperties
    {
        public Difficult Difficult;

        public EnvironmentType EnvironmentType;
        
        public LevelSize Size;

        //Change to model with script prtoperties
        public bool IsLevelWithScript;
    }
}