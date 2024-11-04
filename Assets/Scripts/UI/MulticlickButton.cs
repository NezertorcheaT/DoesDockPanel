using R3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class MulticlickButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UnityEvent leftClick;
        [SerializeField] private UnityEvent middleClick;
        [SerializeField] private UnityEvent rightClick;

        public Observable<Unit> LeftClick => _leftClick;
        public Observable<Unit> MiddleClick => _middleClick;
        public Observable<Unit> RightClick => _rightClick;

        private Subject<Unit> _leftClick = new();
        private Subject<Unit> _middleClick = new();
        private Subject<Unit> _rightClick = new();

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    leftClick?.Invoke();
                    _leftClick.OnNext(new Unit());
                    return;
                case PointerEventData.InputButton.Middle:
                    middleClick?.Invoke();
                    _middleClick.OnNext(new Unit());
                    return;
                case PointerEventData.InputButton.Right:
                    rightClick?.Invoke();
                    _rightClick.OnNext(new Unit());
                    return;
            }
        }
    }
}