using UnityEngine;

namespace Common.UI.Element
{
    public abstract class BaseUISizeProvider : MonoBehaviour
    {
        public Vector2 Size => Vector2.Scale(transform.localScale, BaseSize);
        protected abstract Vector2 BaseSize { get; }
    }
}