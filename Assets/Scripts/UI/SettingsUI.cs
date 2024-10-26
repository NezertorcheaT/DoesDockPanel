using System;
using R3;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using Saving;
using UnityEngine.EventSystems;

namespace UI
{
    public class SettingsUI : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Image trigger;
        [SerializeField] private RectTransform container;
        public Observable<Unit> OnEntered { get; private set; }

        private Subject<Unit> _onEntered;
        private bool _opened;
        private IDisposable _disposableEvents;

        private void OnEnable()
        {
            _onEntered = new Subject<Unit>();
            var disposer = Disposable.CreateBuilder();

            OnEntered = _onEntered
                    .Where(_ => !_opened)
                    .Do(_ => _opened = true)
                ;
            OnEntered.Subscribe(_ => Open()).AddTo(ref disposer);

            closeButton.onClick.AsObservable()
                .Where(_ => _opened)
                .Do(_ => _opened = false)
                .Subscribe(_ => Close()).AddTo(ref disposer);

            _disposableEvents = disposer.Build();
        }

        private void Start()
        {
            Close();
        }

        private void Open()
        {
            Tween.UIAnchoredPosition(container,
                new TweenSettings<Vector2>(ConfigEntry.Instance.SettingsPosition, 0.4f, Ease.OutCubic));
        }

        private void Close()
        {
            ConfigEntry.Instance.SettingsPosition = container.anchoredPosition;
            Tween.UIAnchoredPosition(container,
                new TweenSettings<Vector2>(new Vector2(1300, 200), 0.4f, Ease.OutCubic));
        }

        private void OnDestroy()
        {
            if (_opened)
                ConfigEntry.Instance.SettingsPosition = container.anchoredPosition;
            _disposableEvents.Dispose();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.hovered.Contains(trigger.gameObject))
                _onEntered.OnNext(new Unit());
        }
    }
}