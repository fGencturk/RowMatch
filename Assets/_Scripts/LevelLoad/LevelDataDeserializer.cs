using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.Gameplay.Item;
using Game.Model;

namespace LevelLoad
{
    public static class LevelDataDeserializer
    {
        private const string LevelNumberKey = "level_number";
        private const string GridWidthKey = "grid_width";
        private const string GridHeightKey = "grid_height";
        private const string MoveCountKey = "move_count";
        private const string GridKey = "grid";

        private const char KeyValueSeparator = ':';
        private const char GridContentSeparator = ',';

        private static Dictionary<string, ItemType> _keyToItemType = new Dictionary<string, ItemType>()
        {
            { "r", ItemType.Red },
            { "g", ItemType.Green },
            { "b", ItemType.Blue },
            { "y", ItemType.Yellow },
        };

        public static LevelModel LoadFromFile(string path)
        {
            var fileContent = File.ReadAllText(path);
            return LoadFromString(fileContent);
        }

        public static LevelModel LoadFromString(string data)
        {
            var contentDict = new Dictionary<string, string>();
            
            var stringSeparators = new string[] { "\n" };
            var lines = data.Split(stringSeparators, StringSplitOptions.None);
            foreach (var line in lines)
            {
                var contentArray = line.Split(KeyValueSeparator);
                var key = contentArray[0].Trim();
                var value = contentArray[1].Trim();
                contentDict[key] = value;
            }

            var levelData = new LevelModel();

            levelData.LevelNumber = int.Parse(contentDict[LevelNumberKey]);
            levelData.GridWidth = int.Parse(contentDict[GridWidthKey]);
            levelData.GridHeight = int.Parse(contentDict[GridHeightKey]);
            levelData.MoveCount = int.Parse(contentDict[MoveCountKey]);
            levelData.Grid = contentDict[GridKey].Split(GridContentSeparator).Select(key => _keyToItemType[key]).ToArray();

            return levelData;
            
        }
    }
}