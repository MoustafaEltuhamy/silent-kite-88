using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Gameplay
{
    [CreateAssetMenu(fileName = "ScoreConfig", menuName = GameplayCreateAssetsMenuPaths.Root + "ScoreConfig")]
    public class ScoreConfig : ScriptableObject
    {
        [SerializeField] private int matchPoints;
        [SerializeField] private int mismatchPenalty;

        public int MatchPoints => matchPoints;
        public int MismatchPenalty => mismatchPenalty;
    }
}
