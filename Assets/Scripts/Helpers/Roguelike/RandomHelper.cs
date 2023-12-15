using System;
using UnityEngine;

namespace Assets.Scripts.Helpers.Roguelike
{
    public static class RandomHelper
    {
        /// <summary>
        /// Accept float chance (from 0.1 to 1.0) of event and returns bool value of result
        /// </summary>
        public static bool GetResultOfChanceWheel(float chance)
        {
            if (chance > 1.0f || chance < 0.1f)
            {
                throw new ArgumentException($"Chance value ({chance}) must be between 0.1 and 1.0");
            }
            
            return UnityEngine.Random.Range(0.1f, 1.0f) <= chance;
        }
    }
}