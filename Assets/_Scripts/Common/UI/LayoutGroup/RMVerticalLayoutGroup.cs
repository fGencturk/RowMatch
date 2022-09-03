using System;
using System.Collections.Generic;
using Common.UI.Element;
using UnityEngine;

namespace Common.UI.LayoutGroup
{
    public class RMVerticalLayoutGroup : BaseUISizeProvider
    {
        protected override Vector2 BaseSize => _size;

        [SerializeField] private float _SpacingBetween = .1f;
        [SerializeField] private bool _AutoInitialize = true;
        
        private List<BaseUISizeProvider> _elements;
        private Vector2 _size;

        private void Awake()
        {
            if (_AutoInitialize)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            _elements = new List<BaseUISizeProvider>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var childTransform = transform.GetChild(i);
                if (childTransform.TryGetComponent<BaseUISizeProvider>(out var scrollElement))
                {
                    _elements.Add(scrollElement);
                }
                else
                {
                    Debug.LogError($"Root children of RMScrollView must have IRMScrollElement component. {childTransform.name} does not have one.");
                }
            }
            Rebuild();
        }

        public void Rebuild()
        {
            var lastHeight = 0f;
            foreach (var rmuiElement in _elements)
            {
                rmuiElement.transform.localPosition = new Vector3(0, -lastHeight);
                lastHeight += rmuiElement.Size.y + _SpacingBetween;
            }

            _size = new Vector2(_elements.Count > 0 ? _elements[0].Size.x : 0, lastHeight);
        }

    }
}