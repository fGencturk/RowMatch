using Common.UI.Element;
using UnityEngine;

namespace Common.UI
{
    
    [RequireComponent(typeof(BaseUISizeProvider))]
    public class UIPositioner : MonoBehaviour
    {
        [Range(0, 1f)]
        [SerializeField] private float _AnchorX;
        [Range(0, 1f)]
        [SerializeField] private float _AnchorY;

        [SerializeField] private Vector2 _Pivot;
        
        private void Start()
        {
            Rebuild();
        }

        public void Rebuild()
        {
            var uiElement = GetComponent<BaseUISizeProvider>();
            
            // TODO this assumes every element's original pivot in its center;
            var pivotTranslate = new Vector2((.5f - _Pivot.x) * uiElement.Size.x, (.5f - _Pivot.y) * uiElement.Size.y);

            var anchorWorldPosition = UIHelper2D.AnchorToWorldPoint(_AnchorX, _AnchorY);
            transform.position = anchorWorldPosition + pivotTranslate;
        }
    }
}