using System;
using AGAPI.Foundation;

namespace AGAPI.Systems
{
    public class SessionEvents : EventBus<SessionEvents.ISessionEvents>
    {

        /// Interface to tag all Session-related events.
        public interface ISessionEvents : IEvent { }

        public struct OnApplicationQuit : ISessionEvents
        {
        }

        public struct OnApplicationPause : ISessionEvents
        {
            public readonly bool IsPaused;

            public OnApplicationPause(bool isPaused)
            {
                IsPaused = isPaused;
            }
        }

    }
}
