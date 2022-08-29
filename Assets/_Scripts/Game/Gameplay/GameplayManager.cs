using Game.Load;
using Game.Model;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private BoardSlot _BoardSlot;

        private BoardSlot[,] _grid;

        // TODO remove this
        private void Start()
        {
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
            _grid = new BoardSlot[levelModel.GridHeight, levelModel.GridWidth];
            for (var r = 0; r < levelModel.GridHeight; r++)
            {
                for (var c = 0; c < levelModel.GridWidth; c++)
                {
                    var boardSlot = Instantiate(_BoardSlot);
                    var gridIndex = r * levelModel.GridWidth + c;
                    var itemType = levelModel.Grid[gridIndex];
                    boardSlot.Initialize(r, c, itemType);
                    _grid[r, c] = boardSlot;
                }
            }
        }
    }
}