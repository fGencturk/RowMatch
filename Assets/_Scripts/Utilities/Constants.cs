using UnityEngine;

namespace Utilities
{
    public static class Constants
    {
        public static class Gameplay
        {
            public const float BoardSlotWidth = 1f;
            public const float BoardSlotHeight = 1f;
            public static Vector2 BoardSlotSize = new Vector2(BoardSlotWidth, BoardSlotHeight);
            public const float SwipeThreshold = 20;
        }

        public static class Layers
        {
            public const string BoardSlot = "BoardSlot";
        }
    }
}