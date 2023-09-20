using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EventSystem
{
    public interface IGameEventListener
    {
        void OnEventRaised(string parameter = null);
    }
}
