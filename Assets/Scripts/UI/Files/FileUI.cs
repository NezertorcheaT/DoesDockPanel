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
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private RawImage image;

        public Observable<FileObject> Click { get; private set; }
        public Observable<FileObject> DoubleClick { get; private set; }
        public Observable<FileObject> RightClick { get; private set; }

        public const float DoubleClickMaxDelay = 0.2f;

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

            Click = button.onClick
                .AsObservable()
                .TimeInterval()
                .Where(tuple => tuple.Interval.Seconds > DoubleClickMaxDelay)
                .Select(_ => CurrentFile);
            DoubleClick = button.onClick
                .AsObservable()
                .TimeInterval()
                .Where(tuple => tuple.Interval.Seconds <= DoubleClickMaxDelay)
                .Select(_ => CurrentFile);

            CurrentFile = file;
            Image = CurrentFile.Image;
            FileName = Path.GetFileNameWithoutExtension(CurrentFile.File);
            _initialized = true;
        }
    }
}