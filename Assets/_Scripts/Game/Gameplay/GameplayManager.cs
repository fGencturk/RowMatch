using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Context;
using Common.Event;
using Common.Scene.SceneInitializer.Bindings;
using Common.UI;
using Game.Gameplay.Event;
using Game.Gameplay.Item;
using Game.Model;
using UnityEngine;
using Utilities;

namespace Game.Gameplay
{
    public class GameplayManager : MonoBehaviour, IInitializable
    {
        [Header("Prefabs")]
        [SerializeField] private BoardSlot _BoardSlot;
        [SerializeField] private BoardItem _BoardItem;

        [Header("UI Behaviors")] 
        [SerializeField] private BoardSizeProvider _BoardSizeProvider;
        [SerializeField] private UIScaler _UIScaler;
        [SerializeField] private UIPositioner _UIPositioner;

        private BoardSlot[,] _grid;
        private LevelModel _levelModel;
        private Vector2 _boardCenterPoint;
        private int _currentMoveCount;
        private EndGameChecker _endGameChecker;
        private ScoreManager _scoreManager;

        private void Awake()
        {
            _endGameChecker = new EndGameChecker();
        }

        public void Initialize()
        {
            _scoreManager = ProjectContext.GetInstance<ScoreManager>();
        }

        public void StartLevel(LevelModel levelModel)
        {
            _levelModel = levelModel;
            _currentMoveCount = levelModel.MoveCount;
            _grid = new BoardSlot[levelModel.GridHeight, levelModel.GridWidth];

            var gridSize = new Vector2(levelModel.GridWidth, levelModel.GridHeight);
            var centerIndexes = gridSize / 2f;
            var halfBoardSlotSize = Constants.Gameplay.BoardSlotSize / 2f;
            _boardCenterPoint = Vector2.Scale(Constants.Gameplay.BoardSlotSize, centerIndexes) - halfBoardSlotSize;
            for (var r = 0; r < levelModel.GridHeight; r++)
            {
                for (var c = 0; c < levelModel.GridWidth; c++)
                {
                    var boardSlot = Instantiate(_BoardSlot, transform);
                    var gridIndex = r * levelModel.GridWidth + c;
                    
                    var itemType = levelModel.Grid[gridIndex];
                    var boardItem = Instantiate(_BoardItem);
                    boardItem.Initialize(itemType);

                    var boardSlotIndex = new Vector2Int(c, r);
                    boardSlot.Initialize(new Vector2Int(c, r), boardItem);
                    
                    var positionRelativeToBottomLeft = Vector2.Scale(Constants.Gameplay.BoardSlotSize, boardSlotIndex);
                    boardSlot.transform.position = -_boardCenterPoint + positionRelativeToBottomLeft;
                    _grid[r, c] = boardSlot;
                }
            }

            _BoardSizeProvider.GridSize = gridSize;
            _UIScaler.Rebuild();
            _UIPositioner.Rebuild();
            EventManager.Send(LevelStartedEvent.Create(levelModel));
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
            _currentMoveCount--;
            
            // Check for new completed rows
            var completedBoardSlots = GetBoardSlotsToBeCompleted();
            foreach (var completedBoardSlot in completedBoardSlots)
            {
                completedBoardSlot.IsCompleted = true;
            }

            EventManager.Send(PreSwapPerformedEvent.Create(_currentMoveCount, completedBoardSlots));

            var gameEnded = !_endGameChecker.CanMatchAnyRow(_grid, _currentMoveCount);
            if (gameEnded)
            {
                // Send PreGameEndEvent here if needed
                LockAllBoardSlots();
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

            if (gameEnded)
            {
                EventManager.Send(GameEndEvent.Create(_levelModel, _scoreManager.CurrentScore, _scoreManager.PreviousHighScore));
            }
        }

        private void LockAllBoardSlots()
        {
            for (var r = 0; r < _levelModel.GridHeight; r++)
            {
                for (var c = 0; c < _levelModel.GridWidth; c++)
                {
                    _grid[r, c].IsLocked = true;
                }
            }
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