using R3;
using Saving;
using UnityEngine;

namespace UI
{
    public class FolderHorizontalToggle : ToggleUI
    {
        [SerializeField] private BarImages bar;

        private void Start()
        {
            InitWithValue(ConfigEntry.Instance.InnerFolderSide);
            Toggling.Subscribe(args =>
            {
                ConfigEntry.Instance.InnerFolderSide = args.Value;
                bar?.Redraw();
            });
        }
    }
}