using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class FilePop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private FileUI fileUI;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tween.Scale(fileUI.transform,
                new TweenSettings<Vector3>(new Vector3(1.1f, 1.1f, 1.1f), 0.4f, Ease.OutCubic));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tween.Scale(fileUI.transform, new TweenSettings<Vector3>(new Vector3(1, 1, 1), 0.4f, Ease.OutCubic));
        }
    }
}