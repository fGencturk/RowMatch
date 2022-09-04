using System;
using Common.UI.LayoutGroup;
using UnityEngine;

namespace Common.UI.Scroll
{
    public class RMVerticalScrollOcclusion : MonoBehaviour
    {
        [SerializeField] private RMVerticalLayoutGroup _VerticalLayoutGroup;
        [SerializeField] private RMVerticalScrollView _ScrollView;

        private void Awake()
        {
            _ScrollView.OnScroll += OnScroll;
        }

        private void OnScroll(Vector2 position)
        {
            var topYPosition = -position.y;
            var bottomYPosition = topYPosition - _ScrollView.ViewportHeight;
            foreach (var element in _VerticalLayoutGroup.Elements)
            {
                var elementTop = element.transform.localPosition.y;
                var elementBottom = elementTop - element.BaseSize.y;

                if (elementTop < topYPosition && elementTop > bottomYPosition)
                {
                    element.gameObject.SetActive(true);
                }
                else if (elementBottom < topYPosition && elementBottom > bottomYPosition)
                {
                    element.gameObject.SetActive(true);
                }
                else
                {
                    element.gameObject.SetActive(false);
                }
            }
        }
    }
}