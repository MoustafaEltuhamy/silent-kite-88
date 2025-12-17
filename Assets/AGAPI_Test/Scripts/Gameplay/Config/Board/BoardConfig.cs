using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AGAPI.Gameplay
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = GameplayCreateAssetsMenuPaths.Root + "BoardConfig")]
    public class BoardConfig : ScriptableObject
    {
        [SerializeField] private List<Color32> matchableColors;
        [SerializeField] private float cardRevealTimeoutSeconds = 1.0f;

        public List<Color32> MatchableColors => matchableColors;
        public float CardRevealTimeoutSeconds => cardRevealTimeoutSeconds;
    }
}
