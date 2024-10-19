using System.Diagnostics;
using CustomHelper;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace UI
{
    public class BarImages : MonoBehaviour, IEntriable
    {
        [SerializeField] private DockLinks dockLinks;
        [SerializeField] private Transform container;
        [SerializeField] private LinkUI linkPrefab;
        [SerializeField] private FolderUI folderPrefab;

        private void OnEnable()
        {
            dockLinks.Links.ItemAdded += UpdateGUI;
            dockLinks.Links.ItemRemoved += UpdateGUI;
        }

        private void OnDisable()
        {
            dockLinks.Links.ItemAdded -= UpdateGUI;
            dockLinks.Links.ItemRemoved -= UpdateGUI;
        }

        public void Redraw()
        {
            UpdateGUI(dockLinks.Links, null);
        }

        private void UpdateGUI(ObservableList<DockLinks.FileObject> sender,
            ListChangedEventArgs<DockLinks.FileObject> listChangedEventArgs)
        {
            container.ClearKids();
            Helper.FillContainerWithFiles(container, sender, linkPrefab, folderPrefab);
        }

        public void Begin()
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

        public static void OpenWithDefaultProgram(string file)
        {
            using var filerOpener = new Process();

            filerOpener.StartInfo.FileName = "explorer";
            filerOpener.StartInfo.Arguments = file;
            filerOpener.Start();
        }
    }
}