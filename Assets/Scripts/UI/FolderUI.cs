using System.Collections.Generic;
using CustomHelper;
using R3;
using Saving;
using UI;
using UnityEngine;
using UnityEngine.Rendering;

namespace UI
{
    public class FolderUI : FileUI
    {
        public DockLinks.Folder CurrentFolder => CurrentFile as DockLinks.Folder;
        [SerializeField] private Texture2D folderTexture;
        [SerializeField] private LinkUI linkPrefab;
        [SerializeField] private FolderUI folderPrefab;
        [SerializeField] private Transform containerR;
        [SerializeField] private Transform containerL;
        [SerializeField] private Transform containerU;
        [SerializeField] private Transform containerD;

        public void Initialize(DockLinks.Folder folder)
        {
            Initialize(folder as DockLinks.FileObject);
            Image = folderTexture;
            UpdateGUI(CurrentFolder.Links, null);
            OnEnable();
        }

        private void OnEnable()
        {
            if (CurrentFolder is null) return;
            CurrentFolder.Links.ItemAdded += UpdateGUI;
            CurrentFolder.Links.ItemRemoved += UpdateGUI;
        }

        private void OnDisable()
        {
            CurrentFolder.Links.ItemAdded -= UpdateGUI;
            CurrentFolder.Links.ItemRemoved -= UpdateGUI;
        }

        private void UpdateGUI(ObservableList<DockLinks.FileObject> sender,
            ListChangedEventArgs<DockLinks.FileObject> listChangedEventArgs)
        {
            containerR.ClearKids();
            containerL.ClearKids();
            containerU.ClearKids();
            containerD.ClearKids();

            var container = ConfigEntry.Instance.FolderItemsPosition switch
            {
                FolderSide.Up => containerU,
                FolderSide.Down => containerD,
                FolderSide.Right => containerR,
                FolderSide.Left => containerL,
                _ => containerD
            };
            Helper.FillContainerWithFiles(container, sender, linkPrefab, folderPrefab);
        }
    }
}

namespace CustomHelper
{
    public static partial class Helper
    {
        public static void FillContainerWithFiles(Transform container, IEnumerable<DockLinks.FileObject> files,
            LinkUI linkPrefab, FolderUI folderPrefab)
        {
            foreach (var file in files)
            {
                if (file is DockLinks.Link link)
                {
                    var i = Object.Instantiate(linkPrefab, Vector3.zero, Quaternion.identity, container);
                    i.Initialize(link);
                    i.Click.Subscribe(l => Helper.OpenWithDefaultProgram(l.File));
                }

                if (file is DockLinks.Folder folder)
                {
                    var i = Object.Instantiate(folderPrefab, Vector3.zero, Quaternion.identity, container);
                    i.Initialize(folder);
                }
            }
        }
    }
}