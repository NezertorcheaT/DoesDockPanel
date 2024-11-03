using System.Collections.Generic;
using CustomHelper;
using Files;
using Saving.Settings;
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
        [SerializeField] private AdvancedLinkUI advancedLinkPrefab;
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
            _innerUIs.AddRange(Helper.GetFilesForContainer(
                container,
                sender,
                linkPrefab,
                folderPrefab,
                advancedLinkPrefab,
                _insideFolder)
            );
        }
    }
}