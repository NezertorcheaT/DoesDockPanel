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
            button.onClick.AsObservable().Subscribe(Toggle);
        }

        private void Start()
        {
            if (ConfigEntry.Instance.IsVertical)
                On();
            else
                Off();
        }

        private void Toggle(Unit _)
        {
            if (ConfigEntry.Instance.IsVertical)
                Off();
            else
                On();
        }

        private void On()
        {
            ConfigEntry.Instance.IsVertical = true;

            Tween.UIAnchoredPositionX(dotTransform, -23, speed, Ease.InCubic);
            Tween.Color(background, _offColor, speed, Ease.InCubic);

            if (group is VerticalLayoutGroup) return;
            var g = group.gameObject;
            DestroyImmediate(group);
            group = g.AddComponent<VerticalLayoutGroup>();
            var layoutGroup = (VerticalLayoutGroup)group;
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childControlHeight = false;
            layoutGroup.childControlWidth = false;

            group.childAlignment = ConfigEntry.Instance.TextAnchor;
        }

        private void Off()
        {
            ConfigEntry.Instance.IsVertical = false;

            Tween.UIAnchoredPositionX(dotTransform, 23, speed, Ease.InCubic);
            Tween.Color(background, onColor, speed, Ease.InCubic);

            if (group is HorizontalLayoutGroup) return;
            var g = group.gameObject;
            DestroyImmediate(group);
            group = g.AddComponent<HorizontalLayoutGroup>();
            var layoutGroup = (HorizontalLayoutGroup)group;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.childControlHeight = false;
            layoutGroup.childControlWidth = false;

            group.childAlignment = ConfigEntry.Instance.TextAnchor;
        }
    }
}