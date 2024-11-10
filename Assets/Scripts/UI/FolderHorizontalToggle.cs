using Saving.Settings;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class FolderHorizontalToggle : MonoBehaviour
    {
        [Inject] private BarImages _bar;
        [SerializeField] private Toggle toggle;

        private void Start()
        {
            toggle.isOn = ConfigEntry.Instance.InnerFolderSide;
            toggle.onValueChanged.AddListener(args =>
            {
                ConfigEntry.Instance.InnerFolderSide = args;
                _bar?.Redraw();
            });
        }
    }
}