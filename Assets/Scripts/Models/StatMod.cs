using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class StatMod
    {
        public StatName stat;
        public ModifierReciever reciever;
        public StatModifier modifier;
    }
}
