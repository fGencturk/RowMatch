using UnityEngine;

namespace Common.Save
{
    public class PlayerData
    {
        private const string ScorePlayerPrefPrefix = "LevelScore_";

        private string GetPlayerPrefKey(int levelNumber)
        {
            return $"{ScorePlayerPrefPrefix}{levelNumber}";
        }

        public bool TryGetHighScore(int levelNumber, out int score)
        {
            var key = GetPlayerPrefKey(levelNumber);
            score = -1;
            if (!PlayerPrefs.HasKey(key))
            {
                return false;
            }

            score = PlayerPrefs.GetInt(key, 0);
            return score > 0;
        }

        public void SaveHighScore(int levelNumber, int score)
        {
            var key = GetPlayerPrefKey(levelNumber);
            PlayerPrefs.SetInt(key, score);
        }
        
    }
}