
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.EquipmentSystem
{
    public enum ItemType
    {
        // Equipment
        None = 0,
        Helmet = 1,
        Chestplate = 2,
        Gloves = 3,
        Boots = 4,
        Pants = 5,
        Ring = 6,
        Necklase = 7,
        // Weapons
        Sword = 100,
        Knife = 101,
        Bow = 102,
        // Wapon accesory
        Shield = 200,
        MiniCrossbow = 201,
        Quiver = 202,
        

    }

    public static class ItemTypeExtensions
    {
        public static List<ItemType> GetAnalogues(this ItemType type)
        {
            var values = (ItemType[])Enum.GetValues(typeof(ItemType));

            if ((int)type < 100)
            {
                return new List<ItemType> { ItemType.None };
            }
            else if ((int)type < 200) 
            {
                return values
                    .Where(v => (int)v >= 100 && (int)v < 200)
                    .Except(new List<ItemType> { type })
                    .ToList();
            }
            else
            {
                return values
                    .Where(v => (int)v >= 200)
                    .Except(new List<ItemType> { type })
                    .ToList();
            }
        }

        public static ItemType GetComplementary(this ItemType type)
        {
            switch (type)
            {
                case ItemType.Sword: return ItemType.Shield;
                case ItemType.Shield: return ItemType.Sword;
                case ItemType.Knife: return ItemType.MiniCrossbow;
                case ItemType.MiniCrossbow: return ItemType.Knife;
                case ItemType.Bow: return ItemType.Quiver;
                case ItemType.Quiver: return ItemType.Bow;

                default: return ItemType.None;
            }
        }

        public static List<ItemType> GetExcluded(this ItemType type)
        {
            var comp = type.GetAnalogues().ToList();
            comp.AddRange(type.GetComplementary().GetAnalogues());
            return comp;
        }
    }
}
