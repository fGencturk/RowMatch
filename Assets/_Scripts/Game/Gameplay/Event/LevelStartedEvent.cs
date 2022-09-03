using Common.Event;
using Game.Model;

namespace Game.Gameplay.Event
{
    public class LevelStartedEvent : IEvent
    {
        public LevelModel LevelModel { get; }

        private LevelStartedEvent(LevelModel levelModel)
        {
            LevelModel = levelModel;
        }

        public static LevelStartedEvent Create(LevelModel levelModel)
        {
            return new LevelStartedEvent(levelModel);
        }
    }
}