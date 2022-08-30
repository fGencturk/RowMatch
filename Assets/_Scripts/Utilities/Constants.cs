using System.Collections.Generic;
using Common.Enum;
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

        public static class Path
        {
            public static class Level
            {
                public const string LocalPath = "Levels/";
                public const string RemotePath = "https://row-match.s3.amazonaws.com/levels/";
                public static string RemoteCachePath = $"{Application.persistentDataPath}/Levels/";

                public static readonly Dictionary<ContentType, string> ContentTypeToString =
                    new Dictionary<ContentType, string>()
                    {
                        { ContentType.Local, LocalPath },
                        { ContentType.Remote, RemotePath }
                    };
            }
        }

        public static class Scene
        {
            public const string Loader = "Loader";
            public const string Menu = "Menu";
            public const string Gameplay = "Gameplay";
        }

        public static class Tag
        {
            public const string SceneInitializer = "SceneInitializer";
        }
    }
}