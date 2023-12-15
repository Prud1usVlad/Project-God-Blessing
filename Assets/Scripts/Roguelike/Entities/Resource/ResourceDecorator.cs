using System;
using System.Collections.Generic;
using Assets.Scripts.Roguelike.Entities.Player;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Entities.Resource
{
    public class ResourceDecorator : MonoBehaviour, IEntitySpawnDecorator
    {
        public string ResourceParams;

        public float GetSpawnProbability(PlayerDecorator player)
        {
            return 1;
        }
    }
}