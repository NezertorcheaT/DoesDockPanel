using System.Diagnostics;
using System.IO;
using CustomHelper;
using Files;
using Saving.Settings;
using UI.Files;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class BarImages : MonoBehaviour
    {
        [Inject] private DockLinks _dockLinks;
        [SerializeField] private Transform container;
        [SerializeField] private LinkUI linkPrefab;
        [SerializeField] private FolderUI folderPrefab;
        private LayoutGroup _group;

        private void OnEnable()
        {
            _dockLinks.Links.ItemAdded += UpdateGUI;
            _dockLinks.Links.ItemRemoved += UpdateGUI;
        }

        private void OnDisable()
        {
            _dockLinks.Links.ItemAdded -= UpdateGUI;
            _dockLinks.Links.ItemRemoved -= UpdateGUI;
        }

        public void Redraw()
        {
            UpdateGUI(_dockLinks.Links, null);
            if (_group is null || _group.IsDestroyed())
                _group = container.GetComponent<LayoutGroup>();
            _group.childAlignment = ConfigEntry.Instance.TextAnchor;
        }

        private void UpdateGUI(ObservableList<FileObject> sender,
            ListChangedEventArgs<FileObject> listChangedEventArgs)
        {
            container.ClearKids();
            Helper.FillContainerWithFiles(container, sender, linkPrefab, folderPrefab);
        }

        private void Start()
        {
            Redraw();
        }
    }
}

namespace CustomHelper
{
    public static partial class Helper
    {
        public static void ClearKids(this GameObject gameObject) => gameObject.transform.ClearKids();

        public static void ClearKids(this Transform transform)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }

        public static void OpenWithDefaultProgram(FilePath file)
        {
            if (file.IsEmpty) return;
            using var filerOpener = new Process();

            filerOpener.StartInfo.FileName = "explorer";
            filerOpener.StartInfo.Arguments = file.Value
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
            filerOpener.Start();
        }

        public static void OpenWithDefaultProgram(string file) =>
            OpenWithDefaultProgram(new FilePath(file));
    }
}