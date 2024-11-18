using System.IO;
using Files;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Files
{
    public abstract class FileUI : MonoBehaviour
    {
        [SerializeField] private MulticlickButton button;
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private RawImage image;

        public Observable<FileObject> LeftClick => button.LeftClick.Select(_ => CurrentFile);
        public Observable<FileObject> MiddleClick => button.MiddleClick.Select(_ => CurrentFile);
        public Observable<FileObject> RightClick => button.RightClick.Select(_ => CurrentFile);

        public FileObject CurrentFile;
        private bool _initialized;

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

        public void Initialize(FileObject file)
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