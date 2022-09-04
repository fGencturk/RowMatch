using System;
using System.Collections.Generic;
using Common.UI.Element;
using UnityEngine;

namespace Common.UI.LayoutGroup
{
    public class RMVerticalLayoutGroup : BaseUISizeProvider
    {
        public override Vector2 BaseSize => _size;

        [SerializeField] private float _SpacingBetween = .1f;
        [SerializeField] private bool _AutoInitialize = true;
        
        private Vector2 _size;
        
        public List<BaseUISizeProvider> Elements { get; private set; }

        private void Awake()
        {
            if (_AutoInitialize)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            Elements = new List<BaseUISizeProvider>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var childTransform = transform.GetChild(i);
                if (childTransform.TryGetComponent<BaseUISizeProvider>(out var scrollElement))
                {
                    Elements.Add(scrollElement);
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
            foreach (var rmuiElement in Elements)
            {
                rmuiElement.transform.localPosition = new Vector3(0, -lastHeight);
                lastHeight += rmuiElement.BaseSize.y + _SpacingBetween;
            }

            _size = new Vector2(Elements.Count > 0 ? Elements[0].BaseSize.x : 0, lastHeight);
        }

    }
}