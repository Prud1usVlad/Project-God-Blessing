using Assets.Scripts.Roguelike.Entities.Player;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Entities.Resource
{
    public class ResourceDecorator : MonoBehaviour, IEntitySpawnDecorator
    {
        public float GetSpawnProbability(PlayerDecorator player)
        {
            return 1;
        }
    }
}