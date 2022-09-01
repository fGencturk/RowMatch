using System;
using Common.Event;

namespace Common.UI.Window.Event
{
    public class CloseWindowEvent : IEvent
    {
        public Type Type { get; }

        private CloseWindowEvent(Type type)
        {
            Type = type;
        }

        public static CloseWindowEvent Create<T>() where T : Window
        {
            return new CloseWindowEvent(typeof(T));
        }
    }
}