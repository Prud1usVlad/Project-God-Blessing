using System;
using System.Collections.Generic;
using Assets.Scripts.Roguelike.Entities.Player;
using Assets.Scripts.Roguelike.Entities.Resource;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Entities.Enemy
{
    public class EnemyDecorator : MonoBehaviour, IEntitySpawnDecorator
    {
        public string EnemyParams;

        public float GetSpawnProbability(PlayerDecorator player)
        {
            return 1;
        }
    }
}