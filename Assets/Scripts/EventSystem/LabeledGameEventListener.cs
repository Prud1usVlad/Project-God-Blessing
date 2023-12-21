using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EventSystem
{
    public class LabeledGameEventListener : MonoBehaviour, IGameEventListener
    {
        [Tooltip("Event to register with.")]
        public LabeledGameEvent @event;
        public string label;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent<string> response;

        private void OnEnable()
        {
            @event.RegisterListener(this);
        }

        private void OnDisable()
        {
            @event.UnregisterListener(this);
        }

        public void OnEventRaised(string parameter)
        {
            response?.Invoke(parameter);

        }
    }
}
