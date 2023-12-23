using System.Collections.Generic;
using Assets.Scripts.Roguelike.Entities.Enemy;
using Assets.Scripts.Roguelike.Entities.Player;
using Assets.Scripts.Roguelike.Entities.Resource;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Entities.Boss
{
    public class BossDecorator : MonoBehaviour, IEntitySpawnDecorator
    {
        public List<BossParameters> BossParametersList;
        public BossParameters BossParameters;

        public float GetSpawnProbability(PlayerDecorator player)
        {
            return 1;
        }

        public void SetSpawnSetting(PlayerDecorator player)
        {
            BossParameters = ScriptableObject.CreateInstance<BossParameters>();
            BossParameters.Copy(BossParametersList[Random.Range(0, BossParametersList.Count)]);
        }
    }
}