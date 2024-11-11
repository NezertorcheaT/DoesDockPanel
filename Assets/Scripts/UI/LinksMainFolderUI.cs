using System.Linq;
using CustomHelper;
using R3;
using Saving.Settings;
using SFB;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class LinksMainFolderUI : MonoBehaviour
    {
        [Inject] private DockLinks _dock;
        [SerializeField] private Button open;
        [SerializeField] private Button change;

        private void Start()
        {
            _dock.UpdateStarted.Subscribe(DisableButtons);
            _dock.UpdateEnded.Subscribe(EnableButtons);
            open.onClick.AddListener(() => Helper.OpenWithDefaultProgram(ConfigEntry.Instance.LinksPath));
            change.onClick.AddListener(() =>
            {
                DisableButtons();
                StandaloneFileBrowser.OpenFolderPanelAsync(
                    "Select new main links directory",
                    ConfigEntry.Instance.LinksPath,
                    false,
                    strings =>
                    {
                        var linksPath = strings.FirstOrDefault();
                        EnableButtons();

                        if (!string.IsNullOrWhiteSpace(linksPath)) return;
                        ConfigEntry.Instance.LinksPath = linksPath;
                        _dock.UpdateImages();
                    }
                );
            });
        }

        private void DisableButtons(Unit _ = new())
        {
            open.interactable = false;
            change.interactable = false;
        }

        private void EnableButtons(Unit _ = new())
        {
            open.interactable = true;
            change.interactable = true;
        }
    }
}