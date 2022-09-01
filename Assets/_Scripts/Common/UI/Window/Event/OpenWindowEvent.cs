using System;
using Common.Event;

namespace Common.UI.Window.Event
{
    public class OpenWindowEvent : IEvent
    {
        public Type Type { get; }

        private OpenWindowEvent(Type type)
        {
            Type = type;
        }

        public static OpenWindowEvent Create<T>() where T : Window
        {
            return new OpenWindowEvent(typeof(T));
        }
    }
}