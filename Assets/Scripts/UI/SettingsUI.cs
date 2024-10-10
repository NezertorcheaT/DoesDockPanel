using System;
using R3;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using UnityEngine.EventSystems;

namespace UI
{
    public class SettingsUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image trigger;
        [SerializeField] private RectTransform container;
        public Observable<Unit> OnEntered { get; private set; }
        public Observable<Unit> OnExited { get; private set; }

        private Subject<Unit> _onEntered;
        private Subject<Unit> _onExited;
        private bool _opened;
        private IDisposable _disposableEvents;

        private void OnEnable()
        {
            Close();
            _onEntered = new Subject<Unit>();
            _onExited = new Subject<Unit>();
            var disposer = Disposable.CreateBuilder();

            OnEntered = _onEntered
                    .Where(_ => !_opened)
                    .Do(_ => _opened = true)
                ;
            OnEntered.Subscribe(_ => Open()).AddTo(ref disposer);

            OnExited = _onExited
                    .Where(_ => _opened)
                    .Do(_ => _opened = false)
                ;
            OnExited.Subscribe(_ => Close()).AddTo(ref disposer);

            _disposableEvents = disposer.Build();
        }

        private void Open()
        {
            Tween.UIAnchoredPositionX(container,
                new TweenSettings<float>(0f, 0.4f, Ease.InCubic));
        }

        private void Close()
        {
            Tween.UIAnchoredPositionX(container,
                new TweenSettings<float>(200, 0.4f, Ease.InCubic));
        }

        private void OnDestroy()
        {
            _disposableEvents.Dispose();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (
                eventData.hovered.Contains(trigger.gameObject) ||
                eventData.hovered.Contains(container.gameObject)
            )
                _onEntered.OnNext(new Unit());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (
                eventData.hovered.Contains(trigger.gameObject) ||
                eventData.hovered.Contains(container.gameObject)
            )
                _onExited.OnNext(new Unit());
        }
    }
}