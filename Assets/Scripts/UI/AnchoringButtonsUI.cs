using System;
using R3;
using Saving;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AnchoringButtonsUI : MonoBehaviour
    {
        [SerializeField] private GameObject group;
        [SerializeField] private Button Up;
        [SerializeField] private Button Down;
        [SerializeField] private Button Right;
        [SerializeField] private Button Left;
        [SerializeField] private Button RU;
        [SerializeField] private Button RD;
        [SerializeField] private Button LU;
        [SerializeField] private Button LD;
        [SerializeField] private Button Center;
        private IDisposable _disposable;
        private LayoutGroup _group;

        private void Awake()
        {
            var d = Disposable.CreateBuilder();
            SetAnchor(ref d, Up, TextAnchor.UpperCenter);
            SetAnchor(ref d, Down, TextAnchor.LowerCenter);
            SetAnchor(ref d, Right, TextAnchor.MiddleRight);
            SetAnchor(ref d, Left, TextAnchor.MiddleLeft);
            SetAnchor(ref d, RU, TextAnchor.UpperRight);
            SetAnchor(ref d, RD, TextAnchor.LowerRight);
            SetAnchor(ref d, LU, TextAnchor.UpperLeft);
            SetAnchor(ref d, LD, TextAnchor.LowerLeft);
            SetAnchor(ref d, Center, TextAnchor.MiddleCenter);
            _disposable = d.Build();
        }

        private void Start()
        {
            if (_group is null || _group.IsDestroyed())
                _group = group.GetComponent<LayoutGroup>();
            _group.childAlignment = ConfigEntry.Instance.TextAnchor;
        }

        private void SetAnchor(ref DisposableBuilder builder, Button button, TextAnchor anchor)
        {
            button.onClick.AsObservable().Subscribe(_ =>
            {
                if (_group is null || _group.IsDestroyed())
                    _group = group.GetComponent<LayoutGroup>();
                _group.childAlignment = anchor;
                ConfigEntry.Instance.TextAnchor = anchor;
            }).AddTo(ref builder);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}