using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.Stats;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.SaveSystem;
using System.Linq;
using System;
using Assets.Scripts.Models;

namespace Assets.Scripts.StatSystem
{
    [CreateAssetMenu(fileName = "StatsContainer", menuName = "ScriptableObjects/StatSystem/GlobalMods")]
    public class GlobalModifiers : ScriptableObject
    {
        // stats ordered by: reciever >> statName >> source
        public Dictionary<ModifierReciever, Dictionary<StatName, List<StatModifier>>> table;

        public List<StatsContainer> containersToNotify;

        // methods with simplified signature
        public void AddModifiers(CurseCard curse)
        {
            BaseAddRemove(curse.statMods, m => m.reciever, m => m.stat, true);
        }

        public void RemoveModifiers(CurseCard curse)
        {
            BaseAddRemove(curse.statMods, m => m.reciever, m => m.stat, false);
        }

        public void AddModifiers(BonusBuilding building)
        {
            BaseAddRemove(building.statModifiers, m => m.reciever, m => m.stat, true);
        }

        public void RemoveModifiers(BonusBuilding building)
        {
            BaseAddRemove(building.statModifiers, m => m.reciever, m => m.stat, false);
        }

        // basic methods
        public void AddModifiers(IEnumerable<StatMod> modifiers,
            ModifierReciever reciever, StatName stat, bool update = true)
        {
            if (table is null)
                table = new Dictionary<ModifierReciever, Dictionary<StatName, List<StatModifier>>> ();

            if (!table.ContainsKey(reciever))
                table.Add(reciever, new Dictionary<StatName, List<StatModifier>>());

            if (!table[reciever].ContainsKey(stat))
                table[reciever].Add(stat, new List<StatModifier>());

            var list = table[reciever][stat];

            foreach (var modifier in modifiers)
            {
                list.Add(modifier.modifier);
            }

            if (update)
                UpdateContainers(modifiers, true);
        }

        public void RemoveModifiers(IEnumerable<StatMod> modifiers,
            ModifierReciever reciever, StatName stat, bool update = true)
        {
            if (table is null)
                table = new Dictionary<ModifierReciever, Dictionary<StatName, List<StatModifier>>>();

            if (!table.ContainsKey(reciever))
                return;

            if (!table[reciever].ContainsKey(stat))
                return;

            var list = table[reciever][stat];

            foreach (var modifier in modifiers)
            {
                list.Remove(modifier.modifier);
            }

            if (update)
                UpdateContainers(modifiers, false);
        }

        private void UpdateContainers(IEnumerable<StatMod> modifiers, bool add)
        {
            var grupedR = modifiers.GroupBy(m => m.reciever);

            foreach (var grupR in grupedR)
            {
                var grupedS = grupR.GroupBy(m => m.stat);

                var cSet = containersToNotify
                    .Where(c => c.recieverType == grupR.Key);

                foreach (var grupS in grupedS)
                    foreach (var c in cSet)
                    {
                        if (add)
                            c.AddModifiers(grupS.Select(m => m.modifier), grupS.Key);
                        else
                            c.RemoveModifiers(grupS.Select(m => m.modifier));
                    }
                        
            }
        }

        private void BaseAddRemove(List<StatMod> modifiers,
            Func<StatMod, ModifierReciever> recieverSelector,
            Func<StatMod, StatName> statSelector, bool add)
        {
            var gruped = modifiers.GroupBy(recieverSelector);

            foreach (var grup in gruped)
            {
                var innerGruped = grup.GroupBy(statSelector);

                foreach (var iGrup in innerGruped)
                {
                    if (add)
                        AddModifiers(iGrup, grup.Key, iGrup.Key, false);
                    else
                        RemoveModifiers(iGrup, grup.Key, iGrup.Key, false);
                }
            }

            UpdateContainers(modifiers, add);
        }
    }
}
