using System;

namespace Assets.Scripts.Helpers
{
    public static class TagHelper
    {
        public static string RoomSpawnPointTag = "RoomSpawnPoint";

        public static string LegendIconTag = "LegendIcon";

        public static string LegendIconPointTag = "LegendIconPoint";

        public static string MinimapRoomTag = "MinimapRoom";

        public static string MinimapPlayerIconTag = "MinimapPlayerIcon";

        public static string AdditionalCameraTag = "AdditionalCamera";

        public static string UITag = "UI";

        public static string RoomTag = "Room";

        public static string DoorsTag = "Doors";
        public static string WallTag = "Wall";
        public static string PlayerTag = "Player";
        public static string PlayerModelTag = "PlayerModel";
        public static string UITextTag = "UIText";
        public static string MinimapTag = "Minimap";
        public static string PlayerInteractUITextTag = "PlayerInteractUIText";

        public static string HandlersObjectTag = "HandlersObject";
        public static string WheelGadgetTextTag = "WheelGadgetText";
        
        public static class SpawnPointTags
        {
            public static string RoomSpawnPointTag = "RoomSpawnPoint";
            public static string SpawnPointGroupTag = "SpawnPointGroup";
            public static string EnemySpawnPointGroupTag = "EnemySpawnPointGroup";
            public static string BossSpawnPointGroupTag = "BossSpawnPointGroup";
            public static string TreasuresSpawnPointGroupTag = "TreasuresSpawnPointGroup";
            public static string ExitSpawnPointGroupTag = "ExitSpawnPointGroup";
            public static string ResourcesSpawnPointGroupTag = "ResourcesSpawnPointGroup";
            public static string EnemySpawnPointTag = "EnemySpawnPoint";
            public static string BossSpawnPointTag = "BossSpawnPoint";
            public static string TreasuresSpawnPointTag = "TreasuresSpawnPoint";
            public static string ExitSpawnPointTag = "ExitSpawnPoint";
            public static string ResourcesSpawnPointTag = "ResourcesSpawnPoint";
            public static string WallSpawnPointTag = "WallSpawnPoint";
            public static string PlayerSpawnPointTag = "PlayerSpawnPoint";
        }

        public static class ColliderTags
        {
            public static string PlayerInteractColliderTag = "PlayerInteractCollider";
            public static string GadgetDamageAreaTag = "GadgetDamageArea";
            public static string SwordDamageAreaTag = "SwordDamageArea";
            public static string EnemyDamageAreaTag = "EnemyDamageArea";
            public static string ThrowObjectColliderTag = "ThrowObjectCollider";
            public static string PlayerHitboxColliderTag = "PlayerHitboxCollider";
        }
    
        public static class UITags
        {
            public static string PlayerUITag = "PlayerUI";
            public static class Screens
            {
                public static string LeftLevelScreenTag = "LeftLevelScreen";
            }
        }
    }
}