using System;
using System.Collections.Generic;

namespace Common.Event
{
    public static class EventManager
    {
        private class EventHandler<T> where T : IEvent
        {
            public List<Action<T>> Subscribers;

            public static EventHandler<T> Instance
            {
                get => GetInstance();
            }

            private static EventHandler<T> GetInstance()
            {
                if (_instance == null)
                {
                    _instance = new EventHandler<T>();
                }

                return _instance;
            }

            private static EventHandler<T> _instance;
            
            private EventHandler()
            {
                Subscribers = new List<Action<T>>();
            }

            public void FireEvent(T eventData)
            {
                for (var i = Subscribers.Count; i >= 0; i--)
                {
                    Subscribers[i].Invoke(eventData);
                }
            }
        }

        public static void Register<T>(Action<T> listener) where T : IEvent
        {
            EventHandler<T>.Instance.Subscribers.Add(listener);
        }

        public static void Unregister<T>(Action<T> listener) where T : IEvent
        {
            EventHandler<T>.Instance.Subscribers.Remove(listener);
        }

    }
}