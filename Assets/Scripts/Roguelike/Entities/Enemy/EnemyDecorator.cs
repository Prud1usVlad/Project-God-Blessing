using System;
using System.Collections.Generic;
using Assets.Scripts.Roguelike.Entities.Player;
using Assets.Scripts.Roguelike.Entities.Resource;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Roguelike.Entities.Enemy
{
    public class EnemyDecorator : MonoBehaviour, IEntitySpawnDecorator
    {
        public List<EnemyParameters> EnemyParametersList;
        public EnemyParameters EnemyParameters;

        public Action EnemyDeath;

        public float GetSpawnProbability(PlayerDecorator player)
        {
            return 1;
        }

        public void SetSpawnSetting(PlayerDecorator player)
        {
            EnemyParameters = ScriptableObject.CreateInstance<EnemyParameters>();
            EnemyParameters.Copy(EnemyParametersList[Random.Range(0, EnemyParametersList.Count)]);
        }
    }
}