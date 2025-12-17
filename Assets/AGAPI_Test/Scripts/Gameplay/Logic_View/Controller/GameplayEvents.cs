using System.Collections;
using System.Collections.Generic;
using AGAPI.Foundation;
using UnityEngine;


namespace AGAPI.Gameplay
{
    public class GameplayEvents : EventBus<GameplayEvents.IGameplayEvents>
    {

        /// Interface to tag all Gameplay -related events.
        public interface IGameplayEvents : IEvent { }

        public struct OnLevelStarts : IGameplayEvents
        {
        }
        public struct OnLevelCompleted : IGameplayEvents
        {
        }
        public struct OnScoreUpdate : IGameplayEvents
        {
            public readonly int NewScore;

            public OnScoreUpdate(int newScore)
            {
                NewScore = newScore;
            }
        }
    }

}
