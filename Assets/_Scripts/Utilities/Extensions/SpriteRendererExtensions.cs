using UnityEngine;

namespace Utilities.Extensions
{
    public static class SpriteRendererExtensions
    {
        public static void SetAlpha(this SpriteRenderer spriteRenderer, float a)
        {
            var color = spriteRenderer.color;
            color.a = a;
            spriteRenderer.color = color;
        }
    }
}