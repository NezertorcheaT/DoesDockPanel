using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class CercularImages : MonoBehaviour
    {
        [SerializeField] private float offsetToCenter;
        private RectTransform _rectTransform;
        private List<RectTransform> _children;

        public RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        private void OnCanvasGroupChanged()
        {
            UpdateChildren();
            Recalculate();
        }

        private void OnCanvasHierarchyChanged()
        {
            UpdateChildren();
            Recalculate();
        }

        private void OnRectTransformDimensionsChange() => Recalculate();
        private void OnValidate() => Recalculate();
        private void OnEnable() => Recalculate();

        private void UpdateChildren()
        {
            _children = new List<RectTransform>(RectTransform.childCount);
            for (var i = 0; i < RectTransform.childCount; i++)
            {
                var child = RectTransform.GetChild(i) as RectTransform;
                if (child == RectTransform) continue;
                _children.Add(child);
            }
        }

        private void Update()
        {
            if (_children.Count != RectTransform.childCount)
                UpdateChildren();
        }

        private void Recalculate()
        {
            if (_children is null || _children.Count != RectTransform.childCount)
                UpdateChildren();
            var angle = 1f / _children.Count * 360f;
            var offset =
                _children
                    .Select((i, ind) =>
                        i.sizeDelta.magnitude
                    ).Aggregate((a, b) => a + b)
                / _children.Count / (2f * Mathf.Sin(Mathf.PI / _children.Count));
            var i = 0;

            foreach (var child in _children)
            {
                child.anchoredPosition = (Vector2)(Quaternion.Euler(0, 0, angle * i) * Vector2.right) *
                                         (offset - offsetToCenter);
                i++;
            }
        }
    }
}