using System;
using Assets.Scripts.EquipmentSystem;
using UnityEngine;

[Serializable]
public class TreasureData
{
    [Range(1, 25)]
    public int MinItemLevel;
    [Range(1, 25)]
    public int MaxItemLevel;
    public ItemType ItemType;
    [Range(0.01f, 1f)]
    public float LootChanse;
}
