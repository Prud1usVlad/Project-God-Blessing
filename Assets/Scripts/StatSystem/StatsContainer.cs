using Assets.Scripts.Helpers.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Stats
{
    [CreateAssetMenu(fileName = "StatsContainer", menuName = "ScriptableObjects/StatSystem/Stats")]
    public class StatsContainer : ScriptableObject
    {
        public HashSet<StatName> statsPresent;

        [SerializeField]
        private List<Stat> stats;

        public ModifierReciever recieverType;

        public List<Stat> Stats => stats;

        private void OnEnable()
        {
            statsPresent = stats.Select(x => x.name).ToHashSet();
        }

        public Stat GetStat(StatName name)
        {
            return stats.First(x => x.name == name);
        }

        public float GetStatValue(StatName name)
        {
            return GetStat(name).Value;
        }

        public bool HasStat(StatName name)
        {
            return statsPresent.Contains(name);
        }

        public void AddModifiers(IEnumerable<StatModifier> mods, 
            StatName stat)
        {
            if (statsPresent.Contains(stat))
            {
                var s = GetStat(stat);
            
                foreach(var m in mods) 
                {
                    s.RemoveAllModifiersFromSource(m.Source);
                    s.AddModifier(m);
                }
            } 
        }

        public void RemoveModifiers(IEnumerable<StatModifier> mods)
        {
            foreach (var m in mods)
            {
                foreach (var s in stats)
                {
                    s.RemoveModifier(m);
                }
            }
        }
    }
}
