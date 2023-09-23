using Assets.Scripts.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Assets.Scripts.Stats
{
	[Serializable]
	public class Stat
	{
		public StatName name;
		public float baseValue;

		protected bool isDirty = true;
		protected float lastBaseValue;

		protected float _value;
		public virtual float Value {
			get {
				if(isDirty || lastBaseValue != baseValue) {
					lastBaseValue = baseValue;
					_value = CalculateFinalValue();
					isDirty = false;
				}
				return _value;
			}
		}

		protected readonly List<StatModifier> statModifiers;
		public readonly ReadOnlyCollection<StatModifier> modifiers;

		public Stat()
		{
			statModifiers = new List<StatModifier>();
			modifiers = statModifiers.AsReadOnly();
		}

		public Stat(float baseValue) : this()
		{
			this.baseValue = baseValue;
		}

		public virtual void AddModifier(StatModifier mod)
		{
			isDirty = true;
			statModifiers.Add(mod);
		}

		public virtual bool RemoveModifier(StatModifier mod)
		{
			if (statModifiers.Remove(mod))
			{
				isDirty = true;
				return true;
			}
			return false;
		}

		public virtual bool RemoveAllModifiersFromSource(object source)
		{
			int numRemovals = statModifiers.RemoveAll(mod => mod.Source == source);

			if (numRemovals > 0)
			{
				isDirty = true;
				return true;
			}
			return false;
		}

		protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
		{
			if (a.Order < b.Order)
				return -1;
			else if (a.Order > b.Order)
				return 1;
			return 0; //if (a.Order == b.Order)
		}
		
		protected virtual float CalculateFinalValue()
		{
			float finalValue = baseValue;
			float sumPercentAdd = 0;

			statModifiers.Sort(CompareModifierOrder);

			for (int i = 0; i < statModifiers.Count; i++)
			{
				StatModifier mod = statModifiers[i];

				if (mod.Type == ModifierType.Flat)
				{
					finalValue += mod.Value;
				}
				else if (mod.Type == ModifierType.PercentAdd)
				{
					sumPercentAdd += mod.Value;

					if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != ModifierType.PercentAdd)
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

			// Workaround for float calculation errors, like displaying 12.00001 instead of 12
			return (float)Math.Round(finalValue, 3);
		}
	}
}
