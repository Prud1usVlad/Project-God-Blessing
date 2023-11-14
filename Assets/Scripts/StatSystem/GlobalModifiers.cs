using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.Stats;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.SaveSystem;
using System.Linq;
using System;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using System.Text.RegularExpressions;
using UnityEditor.Experimental.GraphView;

namespace Assets.Scripts.StatSystem
{
    [CreateAssetMenu(fileName = "StatsContainer", menuName = "ScriptableObjects/StatSystem/GlobalMods")]
    public class GlobalModifiers : ScriptableObject
    {
        // stats ordered by: reciever >> statName >> source
        public Dictionary<ModifierReciever, Dictionary<StatName, List<StatModifier>>> statTable;
        public Dictionary<ResourceName, List<ResourceModifier>> resTable;

        public List<StatsContainer> statContainers;
        public List<ResourceContainer> resContainers;

        // methods with simplified signature
        public void AddModifiers(ModifiersContainer container)
        {
            BaseAddRemove(container.statModifiers, m => m.reciever, m => m.stat, true);
            BaseAddRemove(container.resourceModifiers, m => m.resource, true);
        }

        public void RemoveModifiers(ModifiersContainer container)
        {
            BaseAddRemove(container.statModifiers, m => m.reciever, m => m.stat, false);
            BaseAddRemove(container.resourceModifiers, m => m.resource, false);
        }

        // basic methods
        public void AddModifiers(IEnumerable<StatMod> modifiers,
            ModifierReciever reciever, StatName stat, bool update = true)

        {
            if (statTable is null)
                statTable = new Dictionary<ModifierReciever, Dictionary<StatName, List<StatModifier>>> ();

            if (!statTable.ContainsKey(reciever))
                statTable.Add(reciever, new Dictionary<StatName, List<StatModifier>>());

            if (!statTable[reciever].ContainsKey(stat))
                statTable[reciever].Add(stat, new List<StatModifier>());

            var list = statTable[reciever][stat];

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
            if (statTable is null)
                statTable = new Dictionary<ModifierReciever, Dictionary<StatName, List<StatModifier>>>();

            if (!statTable.ContainsKey(reciever))
                return;

            if (!statTable[reciever].ContainsKey(stat))
                return;

            var list = statTable[reciever][stat];

            foreach (var modifier in modifiers)
            {
                list.Remove(modifier.modifier);
            }

            if (update)
                UpdateContainers(modifiers, false);
        }

        public void AddModifiers(IEnumerable<ResMod> modifiers,
            ResourceName res, bool update = true)
        {
            if (resTable is null)
                resTable = new Dictionary<ResourceName, List<ResourceModifier>>();

            if (!resTable.ContainsKey(res))
                resTable.Add(res, new List<ResourceModifier>());

            var list = resTable[res];

            foreach (var modifier in modifiers)
            {
                list.Add(modifier.modifier);
            }

            if (update)
                UpdateContainers(modifiers, true);
        }

        public void RemoveModifiers(IEnumerable<ResMod> modifiers,
            ResourceName res, bool update = true)
        {
            if (resTable is null)
                resTable = new Dictionary<ResourceName, List<ResourceModifier>>();

            if (!resTable.ContainsKey(res))
                return;

            var list = resTable[res];

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

                var cSet = statContainers
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

        private void UpdateContainers(IEnumerable<ResMod> modifiers, bool add)
        {
            var grouped = modifiers.GroupBy(m => m.resource);

            foreach (var group in grouped)
            {
                foreach (var c in resContainers)
                {
                    foreach (var mod in group)
                    {
                        var res = c.GetResource(group.Key);

                        if (add && mod.forGain)
                            res.AddGainModifier(mod.modifier);
                        else if (add && ! mod.forGain)
                            res.AddSpendModifier(mod.modifier);
                        else if (!add && mod.forGain)
                            res.RemoveGainModifier(mod.modifier);
                        else if (!add && !mod.forGain)
                            res.RemoveSpendModifier(mod.modifier);
                    }
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

        private void BaseAddRemove(List<ResMod> modifiers,
            Func<ResMod, ResourceName> resSelector, bool add)
        {
            var gruped = modifiers.GroupBy(resSelector);

            foreach (var grup in gruped)
            {
                if (add)
                    AddModifiers(grup, grup.Key, false);
                else
                    RemoveModifiers(grup, grup.Key, false);
            }

            UpdateContainers(modifiers, add);
        }
    }
}
