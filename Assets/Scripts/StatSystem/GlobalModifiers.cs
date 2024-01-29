using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.Stats;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;

namespace Assets.Scripts.StatSystem
{
    [CreateAssetMenu(fileName = "StatsContainer", menuName = "ScriptableObjects/StatSystem/GlobalMods")]
    public class GlobalModifiers : ScriptableObject
    {
        private ConditionChecker checker;

        [Tooltip("Interval in seconds beetween conditional modifiers checks")]
        public float conditionCheckerInterval = 1;

        public List<ConditionalModifier> conditionalModifiers;

        // stats ordered by: reciever >> statName >> source
        public Dictionary<ModifierReciever, Dictionary<StatName, List<StatModifier>>> statTable;
        public Dictionary<ModifierReciever, Dictionary<ResourceName, List<ResourceModifier>>> resTable;

        public List<StatsContainer> statContainers;
        public List<ResourceContainer> resContainers;

        public void Awake()
        {
            var g = new GameObject();
            checker = g.AddComponent<ConditionChecker>();
        }

        // methods with simplified signature
        public void AddModifiers(ModifiersContainer container)
        {
            BaseAddRemove(container.statModifiers, m => m.reciever, m => m.stat, true);
            BaseAddRemove(container.resourceModifiers, m => m.reciever, m => m.resource, true);
        }

        public void RemoveModifiers(ModifiersContainer container)
        {
            BaseAddRemove(container.statModifiers, m => m.reciever, m => m.stat, false);
            BaseAddRemove(container.resourceModifiers, m => m.reciever, m => m.resource, false);
        }

        public void AddModifiers(ExtendedModifiersContainer container)
        {
            AddModifiers(container as ModifiersContainer);
            foreach (var modifier in container.conditionalModifiers)
                AddModifiers(modifier);
        }

        public void RemoveModifiers(ExtendedModifiersContainer container)
        {
            RemoveModifiers(container as ModifiersContainer);
            foreach(var modifier in container.conditionalModifiers)
                RemoveModifiers(modifier);
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
            ModifierReciever reciever, ResourceName res, bool update = true)
        {
            if (resTable is null)
                resTable = new();

            if (!resTable.ContainsKey(reciever))
                resTable.Add(reciever, new());

            if (!resTable[reciever].ContainsKey(res))
                resTable[reciever].Add(res, new());

            var list = resTable[reciever][res];

            foreach (var modifier in modifiers)
            {
                list.Add(modifier.modifier);
            }

            if (update)
                UpdateContainers(modifiers, true);
        }

        public void RemoveModifiers(IEnumerable<ResMod> modifiers,
            ModifierReciever reciever, ResourceName res, bool update = true)
        {
            if (resTable is null)
                resTable = new ();

            if (!resTable.ContainsKey(reciever))
                return;

            if (!resTable[reciever].ContainsKey(res))
                return;

            var list = resTable[reciever][res];

            foreach (var modifier in modifiers)
            {
                list.Remove(modifier.modifier);
            }

            if (update)
                UpdateContainers(modifiers, false);
        }

        public void AddModifiers(ConditionalModifier modifier)
        {
            conditionalModifiers.Add(modifier);
        }

        public void RemoveModifiers(ConditionalModifier modifier)
        {
            conditionalModifiers.Remove(modifier);
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
            Func<ResMod, ModifierReciever> recieverSelector,
            Func<ResMod, ResourceName> resSelector, bool add)
        {
            var gruped = modifiers.GroupBy(recieverSelector);

            foreach (var grup in gruped)
            {
                var innerGruped = grup.GroupBy(resSelector);

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

        private class ConditionChecker : MonoBehaviour
        {
            public GlobalModifiers modifiers;

            private void Awake()
            {
                StartCoroutine(ConditionCheckerRoutine());
            }

            private IEnumerator ConditionCheckerRoutine()
            {
                while (true)
                {
                    foreach (var mod in modifiers.conditionalModifiers)
                        mod.ManageModifier();

                    yield return new WaitForSeconds(modifiers.conditionCheckerInterval);
                }
            }
        }
    }
}
