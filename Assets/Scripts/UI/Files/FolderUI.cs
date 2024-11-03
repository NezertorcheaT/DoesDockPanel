using System.Collections.Generic;
using CustomHelper;
using Files;
using R3;
using Saving.Settings;
using UI;
using UI.Files;
using UnityEngine;
using UnityEngine.Rendering;

namespace UI.Files
{
    public class FolderUI : FileUI
    {
        public Folder CurrentFolder => CurrentFile as Folder;
        public IEnumerable<FileUI> InnerUIs => _innerUIs;

        [SerializeField] private Texture2D folderTexture;
        [SerializeField] private LinkUI linkPrefab;
        [SerializeField] private FolderUI folderPrefab;
        [SerializeField] private Transform containerR;
        [SerializeField] private Transform containerL;
        [SerializeField] private Transform containerU;
        [SerializeField] private Transform containerD;
        private List<FileUI> _innerUIs = new();
        private bool _insideFolder;

        public void Initialize(Folder folder, bool insideFolder = false)
        {
            Initialize(folder as FileObject);
            _insideFolder = insideFolder;
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

        private void UpdateGUI(ObservableList<FileObject> sender,
            ListChangedEventArgs<FileObject> listChangedEventArgs)
        {
            containerR.ClearKids();
            containerL.ClearKids();
            containerU.ClearKids();
            containerD.ClearKids();

            var side =
                    !_insideFolder
                        ? ConfigEntry.Instance.FolderItemsPosition
                        : ConfigEntry.Instance.FolderItemsPosition is FolderSide.Down or FolderSide.Up
                            ? ConfigEntry.Instance.InnerFolderSide
                                ? FolderSide.Right
                                : FolderSide.Left
                            : ConfigEntry.Instance.InnerFolderSide
                                ? FolderSide.Up
                                : FolderSide.Down
                ;
            var container = side switch
            {
                FolderSide.Up => containerU,
                FolderSide.Down => containerD,
                FolderSide.Right => containerR,
                FolderSide.Left => containerL,
                _ => containerD
            };
            _innerUIs.Clear();
            _innerUIs.AddRange(Helper.GetFilesForContainer(container, sender, linkPrefab, folderPrefab, _insideFolder));
        }
    }
}

namespace CustomHelper
{
    public static partial class Helper
    {
        public static void FillContainerWithFiles(
            Transform container,
            IEnumerable<FileObject> files,
            LinkUI linkPrefab,
            FolderUI folderPrefab,
            bool insideFolder = true
        )
        {
            foreach (var _ in GetFilesForContainer(container, files, linkPrefab, folderPrefab, insideFolder))
            {
            }
        }

        public static IEnumerable<FileUI> GetFilesForContainer(
            Transform container,
            IEnumerable<FileObject> files,
            LinkUI linkPrefab,
            FolderUI folderPrefab,
            bool insideFolder = true
        )
        {
            foreach (var file in files)
            {
                FileUI i = null;
                if (file is Link link)
                {
                    i = Object.Instantiate(linkPrefab, Vector3.zero, Quaternion.identity, container);
                    i.Initialize(link);
                    i.Click.Subscribe(l => OpenWithDefaultProgram(l.File));
                }

                if (file is Folder folder)
                {
                    var folderUI = Object.Instantiate(folderPrefab, Vector3.zero, Quaternion.identity, container);
                    folderUI.Initialize(folder, !insideFolder);
                    i = folderUI;
                }

                yield return i;
            }
        }
    }
}