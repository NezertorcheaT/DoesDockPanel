using Input;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace UI.Files
{
    public class FolderCercularPop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Inject] private WindowsInputActions _actions;
        [SerializeField] private FolderCercularUI folderUI;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tween.Scale(folderUI.transform, new TweenSettings<float>(1.1f, 0.2f, Ease.OutCubic));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tween.Scale(folderUI.transform, new TweenSettings<float>(1, 0.2f, Ease.OutCubic));
        }
    }
}