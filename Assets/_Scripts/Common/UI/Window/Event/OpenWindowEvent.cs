using System;
using Common.Event;

namespace Common.UI.Window.Event
{
    public class OpenWindowEvent : IEvent
    {
        public Type Type { get; }
        public object Data { get; }

        private OpenWindowEvent(Type type, object data)
        {
            Type = type;
            Data = data;
        }

        public static OpenWindowEvent Create<T>(object data = null) where T : Window
        {
            return new OpenWindowEvent(typeof(T), data);
        }
    }
}