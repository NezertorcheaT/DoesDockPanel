using PrimeTween;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Files
{
    public class FilePop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform imageContainer;
        [SerializeField] private FileUI fileUI;

        private void Start()
        {
            fileUI.LeftClick.Subscribe(_ =>
                Tween
                    .Scale(imageContainer.transform, new TweenSettings<float>(1.2f, 0.1f, Ease.OutElastic))
                    .Chain(Tween.Scale(imageContainer.transform, new TweenSettings<float>(1, 0.1f, Ease.InCubic)))
            );
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tween.Scale(imageContainer.transform, new TweenSettings<float>(1.1f, 0.2f, Ease.OutCubic));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tween.Scale(imageContainer.transform, new TweenSettings<float>(1, 0.2f, Ease.OutCubic));
        }
    }
}