using Game.Load;
using Game.Model;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private BoardSlot _BoardSlot;

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
                    boardSlot.Initialize(new Vector2Int(c, r),itemType);
                    _grid[r, c] = boardSlot;
                }
            }
        }

        public void MoveItem(Vector2Int itemPosition, Vector2Int direction)
        {
            var otherItemPosition = itemPosition + direction;
            
            if (otherItemPosition.x < 0 || otherItemPosition.x >= _levelModel.GridWidth) return;
            if (otherItemPosition.y < 0 || otherItemPosition.y >= _levelModel.GridHeight) return;

            var boardSlot1 = _grid[itemPosition.y, itemPosition.x];
            var boardSlot2 = _grid[otherItemPosition.y, otherItemPosition.x];

            var tempItemType = boardSlot1.ItemType;
            
            // TODO lock input for both items and items that form a row and then, animate
            
            boardSlot1.UpdateVisual(boardSlot2.ItemType);
            boardSlot2.UpdateVisual(tempItemType);
        }
    }
}