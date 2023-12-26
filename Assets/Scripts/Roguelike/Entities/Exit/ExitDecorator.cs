using Assets.Scripts.Roguelike.Entities.Player;
using Assets.Scripts.Roguelike.Entities.Resource;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Entities.Exit
{
    public class ExitDecorator : MonoBehaviour, IEntitySpawnDecorator
    {
        public string ExitParams;

        public float GetSpawnProbability(PlayerDecorator player)
        {
            return 1;
        }

        public void SetSpawnSetting(PlayerDecorator player)
        {
        }
    }
}