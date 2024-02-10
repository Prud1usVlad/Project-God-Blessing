using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Entities.Enemy
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Roguelike/EnemyParameters", fileName = "EnemyParameters")]
    public class EnemyParameters : ScriptableObject
    {
        public int EnemyLevel;
        public string EnemyName;
        public float EnemyBaseHealth;
        public float EnemyBaseDamage;
        public float EnemyBaseAttackSpeed;
        public float EnemyBaseMovementSpeed;
        public float EnemyBaseDisapearingTime;

        public List<AvailableResource> AvailableResourcesInLoot;
        public List<TreasureData> AvailableItemsInLoot;

        public void Copy(EnemyParameters origin)
        {
            EnemyLevel = origin.EnemyLevel;
            EnemyName = origin.EnemyName;
            EnemyBaseHealth = origin.EnemyBaseHealth;
            EnemyBaseDamage = origin.EnemyBaseDamage;
            EnemyBaseAttackSpeed = origin.EnemyBaseAttackSpeed;
            EnemyBaseMovementSpeed = origin.EnemyBaseMovementSpeed;
            EnemyBaseDisapearingTime = origin.EnemyBaseDisapearingTime;
            AvailableResourcesInLoot = new List<AvailableResource>(origin.AvailableResourcesInLoot);
            AvailableItemsInLoot = new List<TreasureData>(origin.AvailableItemsInLoot);
        }
    }
}