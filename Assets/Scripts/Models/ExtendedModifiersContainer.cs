using Assets.Scripts.StatSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class ExtendedModifiersContainer : ModifiersContainer
    {
        public List<ConditionalModifier> conditionalModifiers = new();

        public override void InitSource(object source)
        {
            base.InitSource(source);

            foreach (var mod in conditionalModifiers)
                mod.source = (Object)source;
        }
    }
}
