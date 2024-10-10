using System.IO;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LinkUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private RawImage image;

        public DockLinks.Link Link;
        public Observable<DockLinks.Link> Open { get; private set; }

        private bool _initialized;

        public void Initialize(DockLinks.Link link)
        {
            if (_initialized)
                return;
            Link = link;
            image.texture = Link.Image;
            FileName = Path.GetFileNameWithoutExtension(Link.Path);
            _initialized = true;
        }

        private void Awake()
        {
            Open = button.onClick.AsObservable().Select(_ => Link);
        }

        public string FileName
        {
            get => textContainer.text;
            set => textContainer.SetText(value);
        }
    }
}