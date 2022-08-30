using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Gameplay.Item;
using Game.Model;
using LevelLoad;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private BoardSlot _BoardSlot;
        [SerializeField] private BoardItem _BoardItem;

        // TODO remove static instance
        public static GameplayManager Instance;

        private BoardSlot[,] _grid;
        private LevelModel _levelModel;

        // TODO remove this
        private void Start()
        {
            Instance = this;
            var str = @"level_number: 1
grid_width: 5
grid_height: 7
move_count: 20
grid: b,b,y,b,b,g,y,g,r,b,y,g,r,g,g,b,b,g,b,y,r,r,g,g,y,g,g,y,y,b,y,b,b,y,b";
            var levelModel = LevelDataDeserializer.LoadFromString(str);
            StartLevel(levelModel);
        }
        
        // TODO should level visual creation be here?
        private void StartLevel(LevelModel levelModel)
        {
            _levelModel = levelModel;
            _grid = new BoardSlot[levelModel.GridHeight, levelModel.GridWidth];
            for (var r = 0; r < levelModel.GridHeight; r++)
            {
                for (var c = 0; c < levelModel.GridWidth; c++)
                {
                    var boardSlot = Instantiate(_BoardSlot);
                    var gridIndex = r * levelModel.GridWidth + c;
                    
                    var itemType = levelModel.Grid[gridIndex];
                    var boardItem = Instantiate(_BoardItem);
                    boardItem.Initialize(itemType);
                    
                    boardSlot.Initialize(new Vector2Int(c, r), boardItem);
                    _grid[r, c] = boardSlot;
                }
            }
        }

        public async void RequestSwipe(Vector2Int itemPosition, Vector2Int direction)
        {
            var otherItemPosition = itemPosition + direction;
            
            if (otherItemPosition.x < 0 || otherItemPosition.x >= _levelModel.GridWidth) return;
            if (otherItemPosition.y < 0 || otherItemPosition.y >= _levelModel.GridHeight) return;

            var boardSlot1 = _grid[itemPosition.y, itemPosition.x];
            var boardSlot2 = _grid[otherItemPosition.y, otherItemPosition.x];

            if (!boardSlot1.CanSwipe || !boardSlot2.CanSwipe) return;

            var boardItem1 = boardSlot1.BoardItem;
            var boardItem2 = boardSlot2.BoardItem;
            
            // Logically swipe items
            boardSlot1.SetOwnedBoardItem(boardItem2);
            boardSlot2.SetOwnedBoardItem(boardItem1);
            
            // Check for new completed rows
            var completedBoardSlots = GetBoardSlotsToBeCompleted();
            foreach (var completedBoardSlot in completedBoardSlots)
            {
                completedBoardSlot.IsCompleted = true;
            }
            
            // Animate swipe
            var animationTasks = new List<Task>();
            animationTasks.Add(boardSlot1.AnimateItemToOrigin());
            animationTasks.Add(boardSlot2.AnimateItemToOrigin());

            await Task.WhenAll(animationTasks);
            
            // Animate completed items
            animationTasks.Clear();
            foreach (var completedBoardSlot in completedBoardSlots)
            {
                animationTasks.Add(completedBoardSlot.AnimateComplete());
            }

            await Task.WhenAll(animationTasks);
            
            // TODO increase score
        }

        private List<BoardSlot> GetBoardSlotsToBeCompleted()
        {
            var boardSlotsToBeCompleted = new List<BoardSlot>();
            for (var r = 0; r < _levelModel.GridHeight; r++)
            {
                var canRowBeCompleted = true;
                var firstItemType = _grid[r, 0].ItemType;
                for (var c = 1; c < _levelModel.GridWidth; c++)
                {
                    var boardSlot = _grid[r, c];
                    // If already completed
                    if (boardSlot.IsCompleted)
                    {
                        canRowBeCompleted = false;
                        break;
                    }

                    if (firstItemType != boardSlot.ItemType)
                    {
                        canRowBeCompleted = false;
                        break;
                    }
                }

                if (canRowBeCompleted)
                {
                    var items = Enumerable.Range(0, _grid.GetLength(1))
                        .Select(x => _grid[r, x])
                        .ToArray();
                    boardSlotsToBeCompleted.AddRange(items);
                }
            }

            return boardSlotsToBeCompleted;
        } 
    }
}