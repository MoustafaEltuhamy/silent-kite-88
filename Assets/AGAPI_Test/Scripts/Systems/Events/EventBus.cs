using System;
using System.Collections.Generic;
using System.Linq;

namespace AGAPI.Systems
{
    public abstract class EventBus<TEvent> where TEvent : IEvent
    {
        private readonly Dictionary<Type, HashSet<Delegate>> _localEventDictionary =
            new Dictionary<Type, HashSet<Delegate>>();

        public virtual void Subscribe<T>(Action<T> listener) where T : struct, TEvent
        {
            Type eventType = typeof(T);
            if (!_localEventDictionary.TryGetValue(eventType, out var thisEvent))
            {
                thisEvent = new HashSet<Delegate>();
                _localEventDictionary[eventType] = thisEvent;
            }

            thisEvent.Add(listener);
        }

        public virtual void Unsubscribe<T>(Action<T> listener) where T : struct, TEvent
        {
            Type eventType = typeof(T);
            if (_localEventDictionary.TryGetValue(eventType, out var thisEvent))
            {
                thisEvent.Remove(listener);
            }
        }

        public virtual void Invoke<T>(in T publishedEvent) where T : struct, TEvent
        {
            var eventType = typeof(T);
            if (!_localEventDictionary.TryGetValue(eventType, out var listeners)) return;

            var snapshot = new Delegate[listeners.Count];
            listeners.CopyTo(snapshot);

            for (int i = 0; i < snapshot.Length; i++)
                ((Action<T>)snapshot[i])?.Invoke(publishedEvent);
        }

    }
}
