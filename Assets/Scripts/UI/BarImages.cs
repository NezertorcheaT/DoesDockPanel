using System.Diagnostics;
using CustomHelper;
using R3;
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

        private void UpdateGUI(ObservableList<DockLinks.Link> sender,
            ListChangedEventArgs<DockLinks.Link> listChangedEventArgs)
        {
            container.ClearKids();
            foreach (var link in sender)
            {
                var i = Instantiate(linkPrefab, Vector3.zero, Quaternion.identity, container);
                i.Initialize(link);
                i.Open.Subscribe(l => Helper.OpenWithDefaultProgram(l.Path));
            }
        }

        public void Begin()
        {
            UpdateGUI(dockLinks.Links, null);
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