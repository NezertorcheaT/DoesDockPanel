using System;
using System.IO;
using Files;
using Input;
using JetBrains.Annotations;
using PrimeTween;
using Saving.Settings;
using SFB;
using UI.Files;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class LinkSettings : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private LinkUI link;
        [SerializeField] private LayoutElement element;
        [SerializeField] private RectTransform settings;
        [SerializeField] private RectTransform container;
        [SerializeField] private RectTransform presentation;
        [SerializeField] private Button leftClick;
        [SerializeField] private Button midClick;
        [SerializeField] private Button rightClick;
        [SerializeField] private Vector2 sizeNew;
        [SerializeField] private Vector2 settingsNew;
        [SerializeField] private Vector2 presentationNew;
        [SerializeField, Min(0)] private float duration = 0.2f;
        private bool _opened;
        private bool _pointerIn;
        private bool _pressed;
        private Action OnPressed;

        private static void Listen(Button button, [NotNull] FilePath original, Action<FilePath> setNew, string name)
        {
            button.onClick.AddListener(() =>
            {
                StandaloneFileBrowser.SaveFilePanelAsync(
                    $"Select new {name} links file",
                    !string.IsNullOrWhiteSpace(original)
                        ? original.Value.Replace(Path.GetFileName(original), "lnk")
                        : ConfigEntry.Instance.LinksPath,
                    !string.IsNullOrWhiteSpace(original)
                        ? Path.GetFileName(original)
                        : "",
                    new ExtensionFilter[] { new("Link", "lnk"), new("Any", "*") },
                    s =>
                    {
                        if (string.IsNullOrWhiteSpace(s)) return;
                        setNew?.Invoke(s);
                    }
                );
            });
        }

        private void Start()
        {
            Listen(leftClick, link.Config.LeftClickAction, a => link.Config.LeftClickAction = a, "leftClick");
            Listen(midClick, link.Config.MiddleClickAction, a => link.Config.MiddleClickAction = a, "middleClick");
            Listen(rightClick, link.Config.RightClickAction, a => link.Config.RightClickAction = a, "rightClick");
        }

        private void AnimateOpen()
        {
            if (_opened) return;
            settings.gameObject.SetActive(true);
            Tween.UIPreferredSize(element, new TweenSettings<Vector2>(sizeNew, duration, Ease.OutElastic));
            Tween.UISizeDelta(container, new TweenSettings<Vector2>(sizeNew, duration, Ease.OutElastic));
            Tween.UIAnchoredPosition(
                presentation,
                new TweenSettings<Vector2>(presentationNew, duration, Ease.OutElastic)
            );
            Tween.UIAnchoredPosition(settings,
                new TweenSettings<Vector2>(settingsNew, duration, Ease.OutElastic));
            Tween.Scale(settings, new TweenSettings<float>(1, duration, Ease.OutElastic));
            _opened = true;
        }

        private void AnimateClose()
        {
            if (!_opened) return;
            var size = new Vector2(100, 100);
            Tween.UIPreferredSize(element, new TweenSettings<Vector2>(size, duration, Ease.OutElastic));
            Tween.UISizeDelta(container, new TweenSettings<Vector2>(size, duration, Ease.OutElastic));
            Tween.UIAnchoredPosition(presentation, new TweenSettings<Vector2>(Vector2.zero, duration, Ease.OutElastic));
            Tween.UIAnchoredPosition(settings, new TweenSettings<Vector2>(Vector2.zero, duration, Ease.OutElastic));
            Tween.Scale(settings, new TweenSettings<float>(0, duration, Ease.OutElastic)).OnComplete(delegate
            {
                settings.gameObject.SetActive(false);
            });
            _opened = false;
        }

        private void Update()
        {
            var key = WindowsInput.GetKey(WindowsInput.Keys.LShiftKey);
            if (key && !_pressed)
            {
                _pressed = true;
                OnPressed?.Invoke();
                return;
            }

            if (!key && _pressed)
                _pressed = false;
        }

        private void OnShift()
        {
            if (!_opened)
                AnimateOpen();
            else
                AnimateClose();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPressed += OnShift;
            _pointerIn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPressed -= OnShift;
            _pointerIn = false;
        }

        private void OnDestroy()
        {
            if (_pointerIn)
                OnPressed -= OnShift;
        }
    }
}