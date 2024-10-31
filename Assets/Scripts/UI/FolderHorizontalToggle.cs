using PrimeTween;
using R3;
using Saving;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FolderHorizontalToggle : MonoBehaviour
    {
        [SerializeField] private BarImages bar;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform dotTransform;
        [SerializeField] private Image background;
        [SerializeField] private Color onColor;
        [SerializeField] private float speed;
        private Color _offColor;

        private void OnEnable()
        {
            _offColor = background.color;
            button.onClick.AsObservable().Subscribe(Toggle);
        }

        private void Start()
        {
            if (ConfigEntry.Instance.InnerFolderSide)
                On();
            else
                Off();
        }

        private void Toggle(Unit _)
        {
            if (ConfigEntry.Instance.InnerFolderSide)
            {
                Off();
                OffAction();
            }
            else
            {
                On();
                OnAction();
            }
        }

        private void On()
        {
            Tween.UIAnchoredPositionX(dotTransform, -23, speed, Ease.OutCubic);
            Tween.Color(background, _offColor, speed, Ease.OutCubic);
        }

        private void OffAction()
        {
            ConfigEntry.Instance.InnerFolderSide = false;
            bar?.Redraw();
        }

        private void OnAction()
        {
            ConfigEntry.Instance.InnerFolderSide = true;
            bar?.Redraw();
        }

        private void Off()
        {
            Tween.UIAnchoredPositionX(dotTransform, 23, speed, Ease.OutCubic);
            Tween.Color(background, onColor, speed, Ease.OutCubic);
        }
    }
}