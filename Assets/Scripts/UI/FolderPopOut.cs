using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class FolderPopOut : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private FolderUI folder;

        private void Start()
        {
            foreach (var fileUI in folder.InnerUIs)
            {
                fileUI.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (var fileUI in folder.InnerUIs)
            {
                Tween.Scale(fileUI.transform, new TweenSettings<Vector3>(new Vector3(1, 1, 1), 0.4f, Ease.OutCubic));
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (var fileUI in folder.InnerUIs)
            {
                Tween.Scale(fileUI.transform, new TweenSettings<Vector3>(new Vector3(0, 0, 0), 0.4f, Ease.OutCubic));
            }
        }
    }
}