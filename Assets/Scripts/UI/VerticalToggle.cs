using Saving.Settings;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class VerticalToggle : MonoBehaviour
    {
        [Inject] private BarImages _bar;
        [SerializeField] private Toggle toggle;
        [SerializeField] private GameObject container;
        private LayoutGroup _group;

        private void Start()
        {
            toggle.isOn = ConfigEntry.Instance.IsVertical;
            toggle.onValueChanged.AddListener(OnNext);
        }

        private void OnNext(bool args)
        {
            ConfigEntry.Instance.IsVertical = args;
            if (ReferenceEquals(_group, null) || _group.IsDestroyed())
                _group = container.GetComponent<LayoutGroup>();

            if (args)
            {
                if (_group is VerticalLayoutGroup) return;
                DestroyImmediate(_group);
                _group = container.AddComponent<VerticalLayoutGroup>();

                var layoutGroup = (VerticalLayoutGroup)_group;
                layoutGroup.childForceExpandWidth = true;
                layoutGroup.childForceExpandHeight = false;
                layoutGroup.childControlHeight = false;
                layoutGroup.childControlWidth = false;
            }
            else
            {
                if (_group is HorizontalLayoutGroup) return;
                DestroyImmediate(_group);
                _group = container.AddComponent<HorizontalLayoutGroup>();

                var layoutGroup = (HorizontalLayoutGroup)_group;
                layoutGroup.childForceExpandWidth = false;
                layoutGroup.childForceExpandHeight = true;
                layoutGroup.childControlHeight = false;
                layoutGroup.childControlWidth = false;
            }

            _group.childAlignment = ConfigEntry.Instance.TextAnchor;
            _bar?.Redraw();
        }
    }
}