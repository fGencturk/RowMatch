using System;
using Common.UI.Element;
using UnityEngine;

namespace Common.UI
{
    [RequireComponent(typeof(BaseUISizeProvider))]
    public class UIScaler : MonoBehaviour
    {
        [SerializeField] private float _MaxWidth;
        [SerializeField] private float _MaxHeight;

        private void Start()
        {
            Rebuild();
        }

        public void Rebuild()
        {
            var sizeProvider = GetComponent<BaseUISizeProvider>();
            var currentSize = sizeProvider.Size;
            var contentAspectRatio = currentSize.x / currentSize.y;
            
            var maxSizes = UIHelper2D.ScreenPercentageToWorldSize(_MaxWidth, _MaxHeight);
            var maxSizeAspectRatio = maxSizes.x / maxSizes.y;

            var currentScale = transform.localScale;
            var scaleFactor = 1f;
            
            if (contentAspectRatio > maxSizeAspectRatio)
            {
                scaleFactor = maxSizes.x / currentSize.x;
            }
            else
            {
                scaleFactor = maxSizes.y / currentSize.y;
            }
            
            transform.localScale = currentScale * scaleFactor;
        }
    }
}