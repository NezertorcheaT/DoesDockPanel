using System;
using R3;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using Saving;
using UnityEngine.EventSystems;

namespace UI
{
    public class SettingsUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private RectTransform openButton;
        [SerializeField] private Image trigger;
        [SerializeField] private RectTransform container;
        private Subject<Unit> _onEntered;
        private Subject<Unit> _onExited;
        private bool _opened;
        private bool _hovered;
        private IDisposable _disposableEvents;

        private void OnEnable()
        {
            _onEntered = new Subject<Unit>();
            _onExited = new Subject<Unit>();
            var disposer = Disposable.CreateBuilder();

            _onEntered
                .Where(_ => !_hovered && !_opened)
                .Do(_ => _hovered = true)
                .Subscribe(_ => Tween.Scale(openButton, 1, 0.2f, Ease.OutCubic))
                .AddTo(ref disposer)
                ;
            _onExited
                .Where(_ => _hovered && !_opened)
                .Do(_ => _hovered = false)
                .Subscribe(_ => Tween.Scale(openButton, 0, 0.2f, Ease.OutCubic))
                .AddTo(ref disposer)
                ;

            closeButton.onClick.AsObservable().Subscribe(_ => Close()).AddTo(ref disposer);

            _disposableEvents = disposer.Build();
            openButton.localScale = Vector3.zero;
        }

        private void Start()
        {
            CloseAnim();
        }

        public void Open()
        {
            if (!_opened)
                OpenAnim();
            Tween.Scale(openButton, 0, 0.2f, Ease.OutCubic);
            _opened = true;
            _hovered = false;
        }

        public void Close()
        {
            if (_opened)
                CloseAnim();
            _opened = false;
            _hovered = false;
        }

        private void OpenAnim()
        {
            Tween.UIAnchoredPosition(container,
                new TweenSettings<Vector2>(ConfigEntry.Instance.SettingsPosition, 0.4f, Ease.OutCubic));
        }

        private void CloseAnim()
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
            if (eventData.hovered.Contains(trigger.gameObject) || eventData.hovered.Contains(openButton.gameObject))
                _onEntered.OnNext(new Unit());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.hovered.Contains(trigger.gameObject) || eventData.hovered.Contains(openButton.gameObject))
                _onExited.OnNext(new Unit());
        }
    }
}