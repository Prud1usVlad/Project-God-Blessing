using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Unity.VisualScripting.Member;

namespace Assets.Scripts.ResourceSystem
{
    [Serializable]
    public class ResourceModifier : StatModifier
    {
        public readonly TransactionType Transaction;

        public ResourceModifier(float value, 
            ModifierType type, TransactionType transaction) 
            : this(value, type, transaction, (int)type, null) { }

        public ResourceModifier(float value, 
            ModifierType type, TransactionType transaction, int order) 
            : this(value, type, transaction, order, null) { }

        public ResourceModifier(float value, 
            ModifierType type, TransactionType transaction, object source) 
            : this(value, type, transaction, (int)type, source) { }

        public ResourceModifier(float value, 
            ModifierType type, TransactionType transaction, int order, object source) 
            : base(value, type, order, source)
        {
            Transaction = transaction;
        }
    }
}
