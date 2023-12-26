using Assets.Scripts.Roguelike.Entities.Player;

namespace Assets.Scripts.Roguelike.Entities.Resource
{
    public interface IEntitySpawnDecorator
    {
        public float GetSpawnProbability(PlayerDecorator player);

        public void SetSpawnSetting(PlayerDecorator player);
    }
}