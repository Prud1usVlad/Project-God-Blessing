using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Helpers
{
    public class DropHandler : MonoBehaviour, IDropHandler
    {
        public UnityEvent<GameObject> @event;

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("OnDrop");

            if (eventData.pointerDrag != null) 
            {
                @event?.Invoke(eventData.pointerDrag);
                Debug.Log("Dropped: " + eventData.pointerDrag.name);
            }
        }
    }
}
