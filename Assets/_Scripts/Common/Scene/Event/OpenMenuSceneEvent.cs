using Common.Event;
using Game.Gameplay.Event;

namespace Common.Scene.Event
{
    public class OpenMenuSceneEvent : IEvent
    {

        public bool TriggeredFromGame => GameEndEvent != null;
        public GameEndEvent GameEndEvent { get; }
        
        private OpenMenuSceneEvent(GameEndEvent gameEndEvent)
        {
            GameEndEvent = gameEndEvent;
        }
        
        public static OpenMenuSceneEvent Create(GameEndEvent gameEndEvent = null)
        {
            return new OpenMenuSceneEvent(gameEndEvent);
        }
    }
}