using Common.Context;
using Common.Save;
using Utilities;

namespace Common.Utility
{
    public static class LevelUtilities
    {
        public static bool IsLevelPlayable(int levelNumber)
        {
            var previousLevelNumber = levelNumber - 1;

            if (previousLevelNumber < Constants.Level.MinimumLevelNumber)
            {
                return true;
            }

            var playerData = ProjectContext.GetInstance<PlayerData>();
            var hasPreviousLevelScore = playerData.TryGetHighScore(previousLevelNumber, out var score);
            return hasPreviousLevelScore;
        }
    }
}