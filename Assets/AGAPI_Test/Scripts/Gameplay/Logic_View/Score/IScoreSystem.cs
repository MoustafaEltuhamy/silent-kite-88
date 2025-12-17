using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AGAPI.Gameplay
{
    public interface IScoreSystem
    {
        int Score { get; }
        void ApplyMatchResult(bool isMatch);
        void ResetScore(int initialScore = 0);
    }
}
