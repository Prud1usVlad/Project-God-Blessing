using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Helpers
{
    public class DragAndDropElement : MonoBehaviour, 
        IPointerDownHandler, IBeginDragHandler, 
        IEndDragHandler, IDragHandler
    {
        private Canvas canvas;
        private Vector3 startPosition;

        public CanvasGroup canvasGroup;
        public RectTransform rectTransform;

        public Action beginDrag;
        public Action endDrag;
        public Action drag;

        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 0.4f;
            canvasGroup.blocksRaycasts = false;
            startPosition = rectTransform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            rectTransform.position = startPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
    }
}
