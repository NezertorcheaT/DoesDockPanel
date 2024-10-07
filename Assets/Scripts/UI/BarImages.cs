using CustomHelper;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI
{
    public class BarImages : MonoBehaviour, IEntriable
    {
        [SerializeField] private DockLinks dockLinks;
        [SerializeField] private Transform container;

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
                var image = new GameObject(link.Path);
                var rawImage = image.AddComponent<RawImage>();
                rawImage.texture = link.Image;
                image.transform.SetParent(container);
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
    }
}