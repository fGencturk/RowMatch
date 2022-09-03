using UnityEngine;

namespace Common.UI.Element
{
    public class UISpriteSizeProvider : BaseUISizeProvider
    {
        [SerializeField] private SpriteRenderer _SpriteRenderer;
        [SerializeField] private Vector2 _Padding;

        protected override Vector2 BaseSize => (Vector2)_SpriteRenderer.bounds.size + _Padding;
    }
}