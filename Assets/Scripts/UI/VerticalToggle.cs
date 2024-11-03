using R3;
using Saving;
using Saving.Settings;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class VerticalToggle : ToggleUI
    {
        [Inject] private BarImages _bar;
        [SerializeField] private GameObject container;
        private LayoutGroup _group;

        private void Start()
        {
            InitWithValue(ConfigEntry.Instance.IsVertical);
            Toggling.Subscribe(OnNext);
            OnNext(new ToggleArgs(ConfigEntry.Instance.IsVertical));
        }

        private void OnNext(ToggleArgs args)
        {
            ConfigEntry.Instance.IsVertical = args.Value;
            if (ReferenceEquals(_group, null) || _group.IsDestroyed())
                _group = container.GetComponent<LayoutGroup>();

            if (args.Value)
            {
                if (_group is VerticalLayoutGroup) return;
                DestroyImmediate(_group);
                _group = container.AddComponent<VerticalLayoutGroup>();

                var layoutGroup = (VerticalLayoutGroup)_group;
                layoutGroup.childForceExpandWidth = args.Value;
                layoutGroup.childForceExpandHeight = !args.Value;
                layoutGroup.childControlHeight = false;
                layoutGroup.childControlWidth = false;
            }
            else
            {
                if (_group is HorizontalLayoutGroup) return;
                DestroyImmediate(_group);
                _group = container.AddComponent<HorizontalLayoutGroup>();

                var layoutGroup = (HorizontalLayoutGroup)_group;
                layoutGroup.childForceExpandWidth = args.Value;
                layoutGroup.childForceExpandHeight = !args.Value;
                layoutGroup.childControlHeight = false;
                layoutGroup.childControlWidth = false;
            }

            _group.childAlignment = ConfigEntry.Instance.TextAnchor;
            _bar?.Redraw();
        }
    }
}