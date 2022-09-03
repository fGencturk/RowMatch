using UnityEngine;

namespace Common.InputSystem
{
    public class EventData
    {
        public bool IsConsumed { get; private set; }
        public Vector2 MousePosition { get; private set; }

        public void Consume()
        {
            IsConsumed = true;
        }
        
        public void DelegateBelow()
        {
            IsConsumed = false;
        }

        public void ReInitialize(Vector2 mousePosition)
        {
            MousePosition = mousePosition;
        }
    }
}