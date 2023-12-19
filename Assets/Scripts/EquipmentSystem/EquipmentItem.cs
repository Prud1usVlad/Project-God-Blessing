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
        public Sprite icon;

        public Price sellPrice;
        public Price buyPrice;
        public Price deconstructionPrice;

        public ItemType type;
        [Tooltip("Used to set relations between accesories and weapons")]
        public ItemType complementary = ItemType.None;
        public SlothType slothType;
        public NationName nation;
        [Range(0, 25)]
        public int level;

        public ModifiersContainer modifiers;

        private void OnEnable()
        {
            modifiers?.InitSource(this);
        }

    }
}
