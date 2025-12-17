using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AGAPI.Gameplay
{
    public interface IGameplayController
    {
        void ReportMatchResult(CardData firstCard, CardData secondCard, bool isMatch, int remainingPairs);
    }
}
