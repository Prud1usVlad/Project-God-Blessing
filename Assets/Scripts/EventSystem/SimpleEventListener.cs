using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Scripts.EventSystem
{
    public class SimpleEventListener : IGameEventListener
    {
        public List<Action> actions;

        public SimpleEventListener(Action action) 
        {
            actions = new List<Action>() { action };
        }

        public SimpleEventListener(IEnumerable<Action> actions)
        {
            this.actions = actions.ToList();
        }

        public void AddAction(Action action)
        {
            actions.Add(action);
        }

        public void OnEventRaised(string parameter = null)
        {
            foreach (var a in actions)
                a.Invoke();
        }
    }
}
