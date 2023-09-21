using Assets.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.EventSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/LabeledGameEvent", fileName = "Event")]
    public class LabeledGameEvent : GameEvent
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        protected readonly Dictionary<string, List<IGameEventListener>> labeledEventListeners =
            new Dictionary<string, List<IGameEventListener>>();

        public void Raise(string label, string parameter = null)
        {
            if (labeledEventListeners.ContainsKey(label)) 
            {
                var listeners = labeledEventListeners[label];

                for (int i = listeners.Count - 1; i >= 0; i--)
                    listeners[i].OnEventRaised(parameter);
            }
        }

        public void RegisterListener(IGameEventListener listener, string label)
        {
            if (labeledEventListeners.Keys.Contains(label))
            {
                var listeners = labeledEventListeners[label];

                if (!listeners.Contains(listener))
                {
                    eventListeners.Add(listener);
                    listeners.Add(listener);
                }
            }
            else
            {
                labeledEventListeners.Add(label,
                    new List<IGameEventListener> { listener });
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(IGameEventListener listener, string label)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);

            if (labeledEventListeners.Keys.Contains(label))
            {
                var listeners = labeledEventListeners[label];

                if (!listeners.Contains(listener))
                {
                    eventListeners.Remove(listener);
                }
            }
        }
    }
}
