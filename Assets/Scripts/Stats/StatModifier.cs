using Assets.Scripts.Helpers.Enums;

namespace Assets.Scripts.Stats
{ 

	public class StatModifier
	{
		public readonly float Value;
		public readonly ModifierType Type;
		public readonly int Order;
		public readonly object Source;

		public StatModifier(float value, ModifierType type, int order, object source)
		{
			Value = value;
			Type = type;
			Order = order;
			Source = source;
		}

		public StatModifier(float value, ModifierType type) : this(value, type, (int)type, null) { }

		public StatModifier(float value, ModifierType type, int order) : this(value, type, order, null) { }

		public StatModifier(float value, ModifierType type, object source) : this(value, type, (int)type, source) { }
	}
}
