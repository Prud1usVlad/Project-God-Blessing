using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.ResourceSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RogueLike/TreasureData", fileName = "TreasureData")]
public class TreasureData : ScriptableObject
{
    public EquipmentItem Item;
}
