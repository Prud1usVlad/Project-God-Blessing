using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        }

        public static class TreasureAnimator
        {
            public static string OpenChestTrigger = "OpenChest";
        }
    }
}
