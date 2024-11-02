using R3;
using Saving;
using VContainer;

namespace UI
{
    public class FolderHorizontalToggle : ToggleUI
    {
        [Inject] private BarImages _bar;

        private void Start()
        {
            InitWithValue(ConfigEntry.Instance.InnerFolderSide);
            Toggling.Subscribe(args =>
            {
                ConfigEntry.Instance.InnerFolderSide = args.Value;
                _bar?.Redraw();
            });
        }
    }
}