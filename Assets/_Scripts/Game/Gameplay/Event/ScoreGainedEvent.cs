using Common.Event;

namespace Game.Gameplay.Event
{
    public class ScoreGainedEvent : IEvent
    {
        public int GainedScore { get; }
        public int CurrentScore { get; }

        private ScoreGainedEvent(int gainedScore, int currentScore)
        {
            GainedScore = gainedScore;
            CurrentScore = currentScore;
        }

        public static ScoreGainedEvent Create(int gainedScore, int currentScore)
        {
            return new ScoreGainedEvent(gainedScore, currentScore);
        }
    }
}