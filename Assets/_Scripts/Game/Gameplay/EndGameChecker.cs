using System;
using System.Collections.Generic;
using System.Linq;
using Game.Gameplay.Item;

namespace Game.Gameplay
{
    public class EndGameChecker
    {

        public bool CanMatchAnyRow(BoardSlot[,] grid, int remainingMoveCount)
        {
            if (remainingMoveCount == 0) return false;
            
            var rowCount = grid.GetLength(0);
            var columnCount = grid.GetLength(1);

            var touchingItemTypes = new List<ItemType>();

            for (var r = 0; r < rowCount; r++)
            {
                var isRowCompleted = grid[r, 0].IsCompleted;
                if (isRowCompleted)
                {
                    // Check above subgroup
                    if (RowMatchExistsInSubgroup(touchingItemTypes, columnCount, remainingMoveCount))
                    {
                        return true;
                    }
                    touchingItemTypes.Clear();
                }
                else
                {
                    for (var c = 0; c < columnCount; c++)
                    {
                        touchingItemTypes.Add(grid[r, c].ItemType);
                    }
                }
            }
            
            if (RowMatchExistsInSubgroup(touchingItemTypes, columnCount, remainingMoveCount))
            {
                return true;
            }

            return false;
        }

        private bool RowMatchExistsInSubgroup(List<ItemType> group, int columnCount, int remainingMoveCount)
        {
            // TODO also calculate if possible rows can be reachable with remaining move count
            var itemTypes = (ItemType[])Enum.GetValues(typeof(ItemType));
            
            foreach (var itemType in itemTypes)
            {
                var itemCountInGroup = group.Count(item => item == itemType);
                if (itemCountInGroup >= columnCount) return true;
            }

            return false;
        }
        
    }
}