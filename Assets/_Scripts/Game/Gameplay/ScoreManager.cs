using System.Linq;
using Common.Event;
using Common.Scene.SceneInitializer;
using Game.Gameplay.Event;
using Utilities;

namespace Game.Gameplay
{
    public class ScoreManager : IInitializable
    {
        public bool HighScoreReached { get; private set; }
        public int CurrentScore { get; private set; }
        
        public void Initialize()
        {
            EventManager.Register<PreSwapPerformedEvent>(OnPreSwapPerformedEvent);
        }

        private void OnPreSwapPerformedEvent(PreSwapPerformedEvent data)
        {
            var gainedScore = data.CompletedBoardSlots.Sum(boardSlot => Constants.Gameplay.Score.ItemTypeToScore[boardSlot.ItemType]);
            CurrentScore += gainedScore;
            EventManager.Send<ScoreGainedEvent>(ScoreGainedEvent.Create(gainedScore, CurrentScore));
        }
    }
}