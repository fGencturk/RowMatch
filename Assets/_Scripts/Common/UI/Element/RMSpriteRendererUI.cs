using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Element
{
    public class RMSpriteRendererUI : MonoBehaviour, IRMUIElement
    {
        [SerializeField] private SpriteRenderer _SpriteRenderer;
Button
        public Vector2 Size => _SpriteRenderer.bounds.size;
    }
}