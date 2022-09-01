using Common.Event;

namespace Common.Scene.Event
{
    public class OpenMenuSceneEvent : IEvent
    {
        private OpenMenuSceneEvent() { }

        public static OpenMenuSceneEvent Create()
        {
            return new OpenMenuSceneEvent();
        }
    }
}