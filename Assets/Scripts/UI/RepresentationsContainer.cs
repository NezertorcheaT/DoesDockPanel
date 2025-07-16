using CustomHelper;
using MiddleSpawn;
using UnityEngine;
using VContainer;

namespace UI
{
    public class RepresentationsContainer : MonoBehaviour
    {
        [Inject] private MainContainer _mainContainer;
        [Inject] private OpeningIndexListener _openingIndexListener;
        [SerializeField] private FileRepresentation representationPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private Texture2D emptyFolder;
        private IOpenablesContainer _currentContainer;
        private bool _initialized;

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            UpdateContainer(_mainContainer);
            _openingIndexListener.OnSelected += OnIndexSelected;
        }

        private void OnIndexSelected(OpeningIndex index)
        {
            _currentContainer.UseAt(index, UpdateContainer);
        }

        private void UpdateContainer(IOpenablesContainer newContainer)
        {
            if (newContainer == _currentContainer) return;
            _currentContainer = newContainer;
            container.ClearKids();

            {
                var i = Instantiate(representationPrefab, container);
                i.FilePath = "Previous";
                i.Image = emptyFolder;
                i.Keymap = OpeningIndex.Return();
                i.OnClick += () => UpdateContainer(_currentContainer.Parent);
            }

            foreach (var (index, openable) in _currentContainer)
            {
                var i = Instantiate(representationPrefab, container);
                i.FilePath = openable.CurrentPath;
                i.Image = DockTextures.Get(openable.CurrentPath);
                i.Keymap = index;
                i.OnClick += () =>
                {
                    Debug.Log(index.Index);
                    _currentContainer.UseAt(index, UpdateContainer);
                };
            }
        }

        private void OnDestroy()
        {
            _openingIndexListener.OnSelected -= OnIndexSelected;
        }
    }
}