using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Stats
{
    [CreateAssetMenu(fileName = "StatsContainer", menuName = "ScriptableObjects/Stats")]
    public class StatsContainer : ScriptableObject
    {
        [SerializeField]
        private List<Stat> stats;

        public List<Stat> Stats => stats;

        public Stat GetStat(StatName name)
        {
            return stats.First(x => x.name == name);
        }

        public float GetStatValue(StatName name) 
        {
            return GetStat(name).Value;
        }
    }
}
