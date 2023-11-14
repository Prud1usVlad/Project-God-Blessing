using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Registries
{
    public interface IListViewExtendedRegistry
    {
        public void ForEach(Action<object> action);
        public object Find(string guid);
    }
}
