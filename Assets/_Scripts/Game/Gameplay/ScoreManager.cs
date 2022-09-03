using System.Linq;
using Common.Context;
using Common.Event;
using Common.Save;
using Common.Scene.SceneInitializer.Bindings;
using Game.Gameplay.Event;
using Utilities;

namespace Game.Gameplay
{
    public class ScoreManager : IInitializable, IDisposable
    {
        public bool HighScoreReached { get; private set; }
        public int CurrentScore { get; private set; }

        private int _previousHighScore;
        private PlayerData _playerData;
        private int _currentLevelNumber;

        public void Initialize()
        {
            _playerData = ProjectContext.GetInstance<PlayerData>();
            EventManager.Register<PreSwapPerformedEvent>(OnPreSwapPerformedEvent);
            EventManager.Register<LevelStartedEvent>(OnLevelStarted);
        }

        public void Dispose()
        {
            EventManager.Unregister<PreSwapPerformedEvent>(OnPreSwapPerformedEvent);
            EventManager.Unregister<LevelStartedEvent>(OnLevelStarted);
        }

        private void OnLevelStarted(LevelStartedEvent data)
        {
            _currentLevelNumber = data.LevelModel.LevelNumber;
            _playerData.TryGetHighScore(data.LevelModel.LevelNumber, out _previousHighScore);
            HighScoreReached = false;
        }

        private void OnPreSwapPerformedEvent(PreSwapPerformedEvent data)
        {
            var gainedScore = data.CompletedBoardSlots.Sum(boardSlot => Constants.Gameplay.Score.ItemTypeToScore[boardSlot.ItemType]);
            
            CurrentScore += gainedScore;
            if (CurrentScore > _previousHighScore)
            {
                HighScoreReached = true;
                _playerData.SaveHighScore(_currentLevelNumber, CurrentScore);
            }
            
            EventManager.Send<ScoreGainedEvent>(ScoreGainedEvent.Create(gainedScore, CurrentScore));
        }

    }
}