using UnityEngine;

namespace Common.UI.Element
{
    public interface IRMUIElement
    {
        Transform transform { get; }
        Vector2 Size { get; }
    }
}