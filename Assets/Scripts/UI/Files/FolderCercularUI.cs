using System;
using System.Collections.Generic;
using System.IO;
using CustomHelper;
using Files;
using Saving.Links;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

namespace UI.Files
{
    public class FolderCercularUI : FileUI, IConfigurableFileUI<FolderConfig>, IFolderUI
    {
        [Inject] private Func<IFolderUI, Transform, Folder, bool, IFolderUI> _folderFactory;
        [Inject] private Func<LinkUI, Transform, Link, LinkUI> _linkFactory;

        public FileUI Instance => this;
        public GameObject ContainedIn => gameObject;
        public Folder CurrentFolder => CurrentFile as Folder;
        public IEnumerable<FileUI> InnerUIs => _innerUIs;

        public FilePath ConfigFile => Path.Join(CurrentFolder.File,
            $"{FileObject.ExcludedStarting}.folder.{LinkUI.ConfigExtension}");

        public FolderConfig Config { get; private set; }

        [SerializeField] private Texture2D folderTexture;
        [SerializeField] private LinkUI linkPrefab;
        [SerializeField] private FolderCercularUI folderPrefab;
        [SerializeField] private RectTransform container;
        private List<FileUI> _innerUIs = new();

        public void Initialize(Folder folder, bool insideFolder = false)
        {
            Initialize(folder as FileObject);

            var saver = new FolderConfigFileSaver();
            Config = new FolderConfig(saver, this);
            if (File.Exists(ConfigFile))
                Config = Config.Deconvert(saver.Read(ConfigFile), saver) as FolderConfig;

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
            container.ClearKids();
            container.localPosition += (Vector3)Config.Offset;
            _innerUIs.Clear();
            _innerUIs.AddRange(Helper.GetFilesForContainer(
                container,
                sender,
                linkPrefab,
                folderPrefab,
                _folderFactory,
                _linkFactory,
                false)
            );
        }
    }
}