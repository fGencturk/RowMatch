using System.Collections.Generic;
using Common.UI.Element;
using UnityEngine;

namespace Common.UI.Scroll
{
    public class RMScrollView : MonoBehaviour
    {

        private List<IRMUIElement> _elements;

        private void Awake()
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
        
        
        
    }
}