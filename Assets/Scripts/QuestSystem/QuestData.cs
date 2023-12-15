using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.Helpers;
using Assets.Scripts.QuestSystem.Stages;
using Assets.Scripts.ResourceSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.QuestSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/QuestSystem/QuestData")]
    public class QuestData : SerializableScriptableObject
    {
        public string questName;
        public string description;
        public NationName nation;
        public FameLevel fameLevel;
        [Range(0,25)]
        public int connectionLevel;
        public bool isReplayable;

        [SerializeReference]
        public List<QuestStage> stages;

        // Location to perform

        [Header("Rewards")]
        public int connectionPoints;
        public int famePoints;
        public List<Resource> resources;
        public List<EquipmentItem> equipment;
    }
}
