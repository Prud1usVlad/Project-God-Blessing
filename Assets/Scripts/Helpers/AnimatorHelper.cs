namespace Assets.Scripts.Helpers
{
    public static class AnimatorHelper
    {
        public static class PlayerAnimator
        {
            public static string MovementSpeedParameter = "MovementSpeed";
            public static string IsDashParameter = "IsDash";
            public static string DodgeTrigger = "Dodge";
            public static string MeleeAttackTrigger = "MeleeAttack";
            public static string GadgetTrigger = "GadgetThrowing";
            public static string CollectTrigger = "Collect";
            public static string OpenDoorTrigger = "OpenDoor";
            public static string OpenChestTrigger = "OpenChest";
            public static string DeathTrigger = "Death";
            public static string ReviveTrigger = "Revive";
        }

        public static class TreasureAnimator
        {
            public static string OpenChestTrigger = "OpenChest";
        }

        public static class UIAnimator
        {
            public static string IsHoveredParameter = "IsHovered";
        }

        public static class EnemyAnimator
        {
            public static class OrcAnimator
            {
                public static string IsWalkParameter = "IsWalk";

                public static string PunchTrigger = "Punch";

                public static string DieTrigger = "Die";

            }
            public static class Attack
            {
                public static string PunchTag = "Punch";
                public static string ThrowTag = "Throw";
                public static string SummonTag = "Summon";
                public static string BiteTag = "Bite";
            }
        }
    }
}
