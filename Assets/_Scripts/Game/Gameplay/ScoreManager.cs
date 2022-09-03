using System.Linq;
using Common.Event;
using Common.Scene.SceneInitializer.Bindings;
using Game.Gameplay.Event;
using Utilities;

namespace Game.Gameplay
{
    public class ScoreManager : IInitializable, IDisposable
    {
        public bool HighScoreReached { get; private set; }
        public int CurrentScore { get; private set; }
        
        public void Initialize()
        {
            EventManager.Register<PreSwapPerformedEvent>(OnPreSwapPerformedEvent);
        }
        
        public void Dispose()
        {
            EventManager.Unregister<PreSwapPerformedEvent>(OnPreSwapPerformedEvent);
        }

        private void OnPreSwapPerformedEvent(PreSwapPerformedEvent data)
        {
            var gainedScore = data.CompletedBoardSlots.Sum(boardSlot => Constants.Gameplay.Score.ItemTypeToScore[boardSlot.ItemType]);
            CurrentScore += gainedScore;
            EventManager.Send<ScoreGainedEvent>(ScoreGainedEvent.Create(gainedScore, CurrentScore));
        }

    }
}