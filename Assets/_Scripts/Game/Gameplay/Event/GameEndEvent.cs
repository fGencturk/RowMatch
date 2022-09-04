using Common.Event;
using Game.Model;

namespace Game.Gameplay.Event
{
    public class GameEndEvent : IEvent
    {
        public LevelModel LevelModel { get; }
        public int Score { get; }
        public int PreviousHighScore { get; }
        public bool HighScoreReached => Score > PreviousHighScore;

        private GameEndEvent(LevelModel levelModel, int score, int previousHighScore)
        {
            LevelModel = levelModel;
            Score = score;
            PreviousHighScore = previousHighScore;
        }

        public static GameEndEvent Create(LevelModel levelModel, int score, int previousHighScore)
        {
            return new GameEndEvent(levelModel, score, previousHighScore);
        }
    }
}