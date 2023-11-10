using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.Stats;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ResourceSystem
{
    [Serializable]
    public class Resource
    {
        public int amount = 100;
        public ResourceName name;

        // Modifiers applied while trying to gain resource
        protected readonly List<ResourceModifier> _gainModifiers;
        public readonly ReadOnlyCollection<ResourceModifier> gainModifiers;

        // Modifiers applied while trying to spend resource
        protected readonly List<ResourceModifier> _spendModifiers;
        public readonly ReadOnlyCollection<ResourceModifier> spendModifiers;

        public Resource()
        {
            _gainModifiers = new List<ResourceModifier>();
            gainModifiers = _gainModifiers.AsReadOnly();

            _spendModifiers = new List<ResourceModifier>();
            spendModifiers = _spendModifiers.AsReadOnly();
        }

        public int ValueWithModifiers(int amount, TransactionType transaction, bool gain)
        {
            if (gain)
                return CalcValue(amount, transaction, _gainModifiers);
            else
                return CalcValue(amount, transaction, _spendModifiers);
        }

        public void AddGainModifier(ResourceModifier mod)
        {
            _gainModifiers.Add(mod);
        }

        public void AddSpendModifier(ResourceModifier mod)
        {
            _spendModifiers.Add(mod);
        }

        public void RemoveGainModifier(ResourceModifier mod)
        {
            _gainModifiers.Remove(mod);
        }

        public void RemoveSpendModifier(ResourceModifier mod)
        {
            _spendModifiers.Remove(mod);
        }

        public bool CanAfford(int amount, TransactionType transactionType)
        {
            var price = CalcValue(amount, transactionType, _spendModifiers);

            return this.amount >= price;
        }

        public int SpendResource(int amount, TransactionType transactionType)
        {
            var price = CalcValue(amount, transactionType, _spendModifiers);

            this.amount -= price;
            return price;
        }

        public int GainResource(int amount, TransactionType transactionType)
        {
            var income = CalcValue(amount, transactionType, _gainModifiers);

            this.amount += income;
            return income;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            int numRemovals = _gainModifiers.RemoveAll(mod => mod.Source == source);
            numRemovals += _spendModifiers.RemoveAll(mod => mod.Source == source);

            return numRemovals > 0;
        }

        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0; //if (a.Order == b.Order)
        }

        private int CalcValue(int amount,
            TransactionType transactionType,
            List<ResourceModifier> modifiers)
        {
            float finalValue = amount;
            float sumPercentAdd = 0;

            modifiers.Sort(CompareModifierOrder);

            for (int i = 0; i < modifiers.Count; i++)
            {
                ResourceModifier mod = modifiers[i];

                if (mod.Transaction == transactionType && mod.Transaction != TransactionType.Any)
                    continue;

                if (mod.Type == ModifierType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == ModifierType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= modifiers.Count || modifiers[i + 1].Type != ModifierType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == ModifierType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }

            return Mathf.RoundToInt(finalValue);
        }
    }
}
