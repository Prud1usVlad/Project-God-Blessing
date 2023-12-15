using System;
using System.Collections.Generic;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums.LevelProperties;
using UnityEngine;

namespace Assets.Scripts.Helpers.Roguelike.EntitiesHandlers
{
    public class EntitiesConcentrator : MonoBehaviour
    {
        public List<GameObject> Enemies;

        public List<GameObject> Bosses;

        public List<GameObject> TreasureObjects;

        public List<GameObject> ResourceObjects;

        public List<GameObject> ExitObjects;
    }
}