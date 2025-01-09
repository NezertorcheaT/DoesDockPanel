using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class DragAndDrop : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField] private new RectTransform transform;
        private Vector2 _offset;
        private bool _dragging;

        private void Start()
        {
            transform ??= GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _offset = transform.anchoredPosition - (Vector2)UnityEngine.Input.mousePosition;
            _dragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_dragging) return;
            _dragging = false;
            transform.anchoredPosition = (Vector2)UnityEngine.Input.mousePosition + _offset;
        }

        private void LateUpdate()
        {
            if (!_dragging) return;
            transform.anchoredPosition = (Vector2)UnityEngine.Input.mousePosition + _offset;
        }
    }
}