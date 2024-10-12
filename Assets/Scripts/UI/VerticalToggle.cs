using System;
using PrimeTween;
using R3;
using Saving;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VerticalToggle : MonoBehaviour
    {
        [SerializeField] private LayoutGroup group;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform dotTransform;
        [SerializeField] private Image background;
        [SerializeField] private Color onColor;
        [SerializeField] private float speed;
        private Color _offColor;

        private void OnEnable()
        {
            _offColor = background.color;
            button.onClick.AsObservable().Subscribe(_ =>
            {
                ConfigEntry.Instance.IsVertical = !ConfigEntry.Instance.IsVertical;
                if (ConfigEntry.Instance.IsVertical)
                {
                    Tween.UIAnchoredPositionX(dotTransform, 23, speed, Ease.InCubic);
                    Tween.Color(background, onColor, speed, Ease.InCubic);
                }
                else
                {
                    Tween.UIAnchoredPositionX(dotTransform, -23, speed, Ease.InCubic);
                    Tween.Color(background, _offColor, speed, Ease.InCubic);
                }

                if (ConfigEntry.Instance.IsVertical)
                    group.SetLayoutHorizontal();
                else
                    group.SetLayoutVertical();
            });
        }
    }
}