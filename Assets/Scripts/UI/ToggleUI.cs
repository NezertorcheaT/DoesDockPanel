using System;
using PrimeTween;
using R3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class ToggleUI : MonoBehaviour
    {
        public class ToggleArgs : EventArgs
        {
            public readonly bool Value;
            public ToggleArgs(bool value) : base() => Value = value;
        }

        [Serializable]
        public class ToggleBoolEvent : UnityEvent<bool>
        {
        }

        [Serializable]
        public class ToggleEvent : UnityEvent<ToggleArgs>
        {
        }

        public event EventHandler<ToggleArgs> OnToggle;
        public Observable<ToggleArgs> Toggling => _toggling;

        public bool Value
        {
            get => _value;
            set
            {
                if (value)
                {
                    if (!_value)
                        On();
                }
                else
                {
                    if (_value)
                        Off();
                }
            }
        }

        [SerializeField] private ToggleEvent onToggle;
        [SerializeField] private ToggleBoolEvent onToggleBool;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform dotTransform;
        [SerializeField] private Image background;
        [SerializeField] private Color onColor;
        [SerializeField] private float speed;

        private Color _offColor;
        private bool _value;
        private Subject<ToggleArgs> _toggling = new();

        private void OnEnable()
        {
            _offColor = background.color;
            button.onClick.AsObservable().Subscribe(_ => Toggle());
        }

        public void InitWithValue(bool value)
        {
            _value = value;
            if (value)
                OnView();
            else
                OffView();
        }

        public void On()
        {
            if (_value) return;
            OnView();
            OnAction();
        }

        public void Off()
        {
            if (!_value) return;
            OffView();
            OffAction();
        }

        public void Toggle()
        {
            if (_value)
                Off();
            else
                On();
        }

        private void OnView()
        {
            Tween.UIAnchoredPositionX(dotTransform, -23, speed, Ease.OutCubic);
            Tween.Color(background, _offColor, speed, Ease.OutCubic);
        }

        private void InvokeEvents()
        {
            var args = new ToggleArgs(_value);
            onToggle?.Invoke(args);
            onToggleBool?.Invoke(args.Value);
            OnToggle?.Invoke(this, args);
            _toggling.OnNext(args);
        }

        private void OffAction()
        {
            _value = false;
            InvokeEvents();
        }

        private void OnAction()
        {
            _value = true;
            InvokeEvents();
        }

        private void OffView()
        {
            Tween.UIAnchoredPositionX(dotTransform, 23, speed, Ease.OutCubic);
            Tween.Color(background, onColor, speed, Ease.OutCubic);
        }
    }
}