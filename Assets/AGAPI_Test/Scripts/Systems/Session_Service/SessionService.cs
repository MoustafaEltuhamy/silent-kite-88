
using System;
using AGAPI.Systems;
using UnityEngine;

namespace AGAPI.Systems
{
    public class SessionService : MonoBehaviour
    {

        private SessionEvents _sessionEvents;


        public void Configure(SessionEvents sessionEvents)
        {
            _sessionEvents = sessionEvents;
        }

        private void OnApplicationQuit()
        {
            _sessionEvents?.Invoke(new SessionEvents.OnApplicationQuit());
        }

        private void OnApplicationPause(bool isPaused)
        {
            _sessionEvents?.Invoke(new SessionEvents.OnApplicationPause(isPaused));
        }

    }
}
