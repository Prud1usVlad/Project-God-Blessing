using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EventSystem
{
    public class GameEventListener : MonoBehaviour, IGameEventListener
    {
        [Tooltip("Event to register with.")]
        public GameEvent @event;

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
