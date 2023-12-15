using System;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;

namespace Assets.Scripts.Helpers.Roguelike.EnumHandlers
{
    public static class SizeHandler
    {
        /// <summary>
        /// Method handle value of Level Size enum and translate it in random value in particular range
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetSize(LevelSize size)
        {
            int min = 1, max = 2;
            switch (size)
            {
                case LevelSize.Small:
                    min = 4;
                    max = 8;
                    break;
                case LevelSize.Medium:
                    min = 9;
                    max = 13;
                    break;
                case LevelSize.Large:
                    min = 14;
                    max = 18;
                    break;
                case LevelSize.Epic:
                    min = 19;
                    max = 30;
                    break;
                default:
                    throw new ArgumentException(message: $"This size: {size} does not suppor yet.");
            }


            return UnityEngine.Random.Range(min, max);
        }
    }
}