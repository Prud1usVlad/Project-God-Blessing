using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EquipmentSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EquipmentSystem/Item", fileName = "NewItem")]
    public class EquipmentItem : SerializableScriptableObject
    {
        public string itemName;
        public string description;

        public Price sellPrice;
        public Price buyPrice;
        public Price deconstructionPrice;

        public ItemType type;
        public NationName nation;
        [Range(0, 25)]
        public int level;

        public List<StatMod> modifiers;
    }
}
