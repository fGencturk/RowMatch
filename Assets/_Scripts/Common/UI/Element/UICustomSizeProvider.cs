using UnityEngine;

namespace Common.UI.Element
{
    public class UICustomSizeProvider : BaseUISizeProvider
    {
        [SerializeField] private Vector2 _Size;
        
        protected override Vector2 BaseSize => _Size;
    }
}