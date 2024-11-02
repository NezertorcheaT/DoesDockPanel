using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class DragAndDrop : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField] private RectTransform transform;
        private Vector2 _offset;
        private bool _dragging;

        private void Start()
        {
            transform ??= GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _offset = transform.anchoredPosition - Mouse.current.position.ReadValue();
            _dragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_dragging) return;
            _dragging = false;
            transform.anchoredPosition = Mouse.current.position.ReadValue() + _offset;
        }

        private void LateUpdate()
        {
            if (!_dragging) return;
            transform.anchoredPosition = Mouse.current.position.ReadValue() + _offset;
        }
    }
}