using Common.Event;
using Game.Model;

namespace Common.Scene.Event
{
    public class OpenGameSceneEvent : IEvent
    {
        public LevelModel LevelModel { get; }

        private OpenGameSceneEvent(LevelModel levelModel)
        {
            LevelModel = levelModel;
        }

        public static OpenGameSceneEvent Create(LevelModel levelModel)
        {
            return new OpenGameSceneEvent(levelModel);
        }
    }
}