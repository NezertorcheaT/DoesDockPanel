using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DragAndDrop : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerMoveHandler
    {
        [SerializeField] private RectTransform transform;
        private Vector2 _offset;
        private bool _dragging;

        public void OnPointerDown(PointerEventData eventData)
        {
            _offset = transform.anchoredPosition - eventData.position;
            _dragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _dragging = false;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_dragging) return;
            transform.anchoredPosition = eventData.position + _offset;
        }
    }
}