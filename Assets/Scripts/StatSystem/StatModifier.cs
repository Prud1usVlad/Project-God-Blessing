using Assets.Scripts.Helpers.Enums;
using System;

namespace Assets.Scripts.Stats
{
	[Serializable]
	public class StatModifier
	{
		public float Value;
		public ModifierType Type;
		public int Order;
		public object Source;

		public StatModifier(float value, ModifierType type, int order, object source)
		{
			Value = value;
			Type = type;
			Order = order;
			Source = source;
		}

		public StatModifier(float value, ModifierType type) 
			: this(value, type, (int)type, null) { }

		public StatModifier(float value, ModifierType type, int order) 
			: this(value, type, order, null) { }

		public StatModifier(float value, ModifierType type, object source) 
			: this(value, type, (int)type, source) { }
	}
}
