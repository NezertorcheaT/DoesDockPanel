using R3;
using Saving;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VerticalToggle : ToggleUI
    {
        [SerializeField] private LayoutGroup group;

        private void Start()
        {
            InitWithValue(ConfigEntry.Instance.IsVertical);
            Toggling.Subscribe(args =>
            {
                ConfigEntry.Instance.IsVertical = args.Value;
                var g = group.gameObject;
                if (args.Value)
                {
                    if (group is VerticalLayoutGroup) return;
                    DestroyImmediate(group);
                    group = g.AddComponent<VerticalLayoutGroup>();

                    var layoutGroup = (VerticalLayoutGroup)group;
                    layoutGroup.childForceExpandWidth = args.Value;
                    layoutGroup.childForceExpandHeight = !args.Value;
                    layoutGroup.childControlHeight = false;
                    layoutGroup.childControlWidth = false;
                }
                else
                {
                    if (group is HorizontalLayoutGroup) return;
                    DestroyImmediate(group);
                    group = g.AddComponent<HorizontalLayoutGroup>();

                    var layoutGroup = (HorizontalLayoutGroup)group;
                    layoutGroup.childForceExpandWidth = args.Value;
                    layoutGroup.childForceExpandHeight = !args.Value;
                    layoutGroup.childControlHeight = false;
                    layoutGroup.childControlWidth = false;
                }

                group.childAlignment = ConfigEntry.Instance.TextAnchor;
            });
        }
    }
}