using UnityEngine;

namespace Common.UI.Element
{
    public class RMSpriteRendererUI : MonoBehaviour, IRMUIElement
    {
        [SerializeField] private SpriteRenderer _SpriteRenderer;

        public Vector2 Size => _SpriteRenderer.bounds.size;
    }
}