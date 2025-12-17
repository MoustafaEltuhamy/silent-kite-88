using System.Collections.Generic;
using UnityEngine;


namespace AGAPI.Gameplay
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = GameplayCreateAssetsMenuPaths.Root + "BoardConfig")]
    public class BoardConfig : ScriptableObject
    {
        [SerializeField] private CardVisual cardVisualPrefab;
        [SerializeField] private Color32 cardUpColor;
        [SerializeField] private Color32 cardDownColor;
        [SerializeField] private List<Color32> matchableColors;
        [SerializeField] private float cardRevealTimeoutSeconds = 1.0f;
        [SerializeField] private float initialRevealDuratoin = .5f;

        public CardVisual CardVisualPrefab => cardVisualPrefab;
        public Color32 CardUpColor => cardUpColor;
        public Color32 CardDownColor => cardDownColor;
        public List<Color32> MatchableColors => matchableColors;
        public float CardRevealTimeoutSeconds => cardRevealTimeoutSeconds;
        public float InitialRevealDuratoin => initialRevealDuratoin;
    }
}
