﻿using System.IO;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class FileUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private RawImage image;
        public Observable<DockLinks.FileObject> Click { get; private set; }
        public DockLinks.FileObject CurrentFile;
        private bool _initialized;

        private void Awake()
        {
            Click = button.onClick.AsObservable().Select(_ => CurrentFile);
        }

        public string FileName
        {
            get => textContainer.text;
            set => textContainer.SetText(value);
        }

        public Texture Image
        {
            get => image.texture;
            set => image.texture = value;
        }

        public void Initialize(DockLinks.FileObject file)
        {
            if (_initialized)
                return;
            CurrentFile = file;
            Image = CurrentFile.Image;
            FileName = Path.GetFileNameWithoutExtension(CurrentFile.File);
            _initialized = true;
        }
    }
}