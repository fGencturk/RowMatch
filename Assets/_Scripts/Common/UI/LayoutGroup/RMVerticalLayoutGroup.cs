using System;
using System.Collections.Generic;
using Common.UI.Element;
using UnityEngine;

namespace Common.UI.LayoutGroup
{
    public class RMVerticalLayoutGroup : MonoBehaviour
    {

        [SerializeField] private float _SpacingBetween = .1f;
        
        private List<IRMUIElement> _elements;

        private void Awake()
        {
            Initialize();
            Rebuild();
        }

        public void Initialize()
        {
            _elements = new List<IRMUIElement>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var childTransform = transform.GetChild(i);
                if (childTransform.TryGetComponent<IRMUIElement>(out var scrollElement))
                {
                    _elements.Add(scrollElement);
                }
                else
                {
                    Debug.LogError($"Root children of RMScrollView must have IRMScrollElement component. {childTransform.name} does not have one.");
                }
            }
        }

        public void Rebuild()
        {
            var lastHeight = 0f;
            foreach (var rmuiElement in _elements)
            {
                rmuiElement.transform.position = new Vector3(0, -lastHeight);
                lastHeight -= rmuiElement.Size.y + _SpacingBetween;
            }
        }
        
    }
}