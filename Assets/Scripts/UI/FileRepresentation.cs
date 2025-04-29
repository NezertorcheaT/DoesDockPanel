using System;
using System.IO;
using MiddleSpawn;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FileRepresentation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fileName;
        [SerializeField] private TextMeshProUGUI kaymap;
        [SerializeField] private RawImage image;
        [SerializeField] private Button button;

        public event Action OnClick;

        public string FilePath
        {
            set => fileName.SetText(Path.GetFileNameWithoutExtension(value));
        }

        public OpeningIndex Keymap
        {
            set => kaymap.SetText(value.AsKey().ToString());
        }

        public Texture2D Image
        {
            set => image.texture = value;
        }

        private void Start()
        {
            button.onClick.AddListener(() => OnClick?.Invoke());
        }
    }
}