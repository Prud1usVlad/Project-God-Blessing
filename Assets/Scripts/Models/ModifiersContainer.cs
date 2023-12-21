using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ModifiersContainer
    {
        public List<StatMod> statModifiers = new();
        public List<ResMod> resourceModifiers = new();

        public void InitSource(object source)
        {
            foreach (var mod in statModifiers)
                mod.modifier.Source = source;

            foreach (var mod in resourceModifiers)
                mod.modifier.Source = source;
        }
    }
}
