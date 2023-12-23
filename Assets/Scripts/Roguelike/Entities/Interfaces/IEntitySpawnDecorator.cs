using System;
using System.Collections.Generic;
using Assets.Scripts.Roguelike.Entities.Player;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Entities.Resource
{
    public interface IEntitySpawnDecorator
    {
        public float GetSpawnProbability(PlayerDecorator player);

        public void SetSpawnSetting(PlayerDecorator player);
    }
}