using System.Collections.Generic;
using Common.Event;

namespace Game.Gameplay.Event
{
    // Triggered when swap performed logically, but not visually on screen - before animation
    public class PreSwapPerformedEvent : IEvent
    {
        public int RemainingMoveCount { get; }
        public List<BoardSlot> CompletedBoardSlots { get; }

        private PreSwapPerformedEvent(int remainingMoveCount, List<BoardSlot> completedBoardSlots)
        {
            RemainingMoveCount = remainingMoveCount;
            CompletedBoardSlots = completedBoardSlots;
        }

        public static PreSwapPerformedEvent Create(int remainingMoveCount, List<BoardSlot> completedBoardSlots)
        {
            return new PreSwapPerformedEvent(remainingMoveCount, completedBoardSlots);
        }
    }
}