using System;
using System.Collections.Generic;
using Assets.Scripts.Roguelike.Entities.Player;
using Assets.Scripts.Roguelike.Entities.Resource;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Entities.Boss
{
    public class BossDecorator : MonoBehaviour, IEntitySpawnDecorator
    {
        public string BossParams;

        public float GetSpawnProbability(PlayerDecorator player)
        {
            return 1;
        }
    }
}