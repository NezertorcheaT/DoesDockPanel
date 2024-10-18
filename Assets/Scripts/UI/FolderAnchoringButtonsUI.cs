using System;
using R3;
using Saving;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FolderAnchoringButtonsUI : MonoBehaviour
    {
        [SerializeField] private Button Up;
        [SerializeField] private Button Down;
        [SerializeField] private Button Right;
        [SerializeField] private Button Left;
        private IDisposable _disposable;

        private void Awake()
        {
            var d = Disposable.CreateBuilder();
            SetAnchor(ref d, Up, FolderSide.Up);
            SetAnchor(ref d, Down, FolderSide.Down);
            SetAnchor(ref d, Right, FolderSide.Right);
            SetAnchor(ref d, Left, FolderSide.Left);
            _disposable = d.Build();
        }

        private void SetAnchor(ref DisposableBuilder builder, Button button, FolderSide anchor)
        {
            button.onClick.AsObservable().Subscribe(_ => { ConfigEntry.Instance.FolderItemsPosition = anchor; })
                .AddTo(ref builder);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}