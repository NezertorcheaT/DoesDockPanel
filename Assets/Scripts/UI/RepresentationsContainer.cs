using CustomHelper;
using MiddleSpawn;
using UnityEngine;
using VContainer;

namespace UI
{
    public class RepresentationsContainer : MonoBehaviour
    {
        [Inject] private MainContainer _mainContainer;
        [SerializeField] private FileRepresentation representationPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private Texture2D emptyFolder;
        private IOpenablesContainer _currentContainer;

        private void Start()
        {
            UpdateContainer(_mainContainer);
        }

        private void UpdateContainer(IOpenablesContainer newContainer)
        {
            _currentContainer = newContainer;
            container.ClearKids();

            {
                var i = Instantiate(representationPrefab, container);
                i.FilePath = "Previous";
                i.Image = emptyFolder;
                i.Keymap = OpeningIndex.Return();
                i.OnClick += () => { UpdateContainer(_currentContainer.Parent); };
            }

            foreach (var (index, openable) in _currentContainer)
            {
                var i = Instantiate(representationPrefab, container);
                i.FilePath = openable.CurrentPath;
                i.Image = DockTextures.Textures[openable.CurrentPath];
                i.Keymap = index;
                i.OnClick += () =>
                {
                    Debug.Log(index.Index);
                    _currentContainer.UseAt(index, UpdateContainer);
                };
            }
        }
    }
}