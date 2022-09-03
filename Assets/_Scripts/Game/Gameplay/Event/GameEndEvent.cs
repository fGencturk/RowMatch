using Common.Event;
using Game.Model;

namespace Game.Gameplay.Event
{
    public class GameEndEvent : IEvent
    {
        public LevelModel LevelModel { get; }
        public int Score { get; }
        public bool HighScoreReached { get; }

        private GameEndEvent(LevelModel levelModel, int score, bool highScoreReached)
        {
            LevelModel = levelModel;
            Score = score;
            HighScoreReached = highScoreReached;
        }

        public static GameEndEvent Create(LevelModel levelModel, int score, bool highScoreReached)
        {
            return new GameEndEvent(levelModel, score, highScoreReached);
        }
    }
}