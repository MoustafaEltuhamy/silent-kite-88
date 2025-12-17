using System.Collections.Generic;
using UnityEngine;


namespace AGAPI.Gameplay
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = GameplayCreateAssetsMenuPaths.Root + "BoardConfig")]
    public class BoardConfig : ScriptableObject
    {
        [SerializeField] private List<Vector2Int> availableBoardSizes;
        [SerializeField] private CardVisual cardVisualPrefab;
        [SerializeField] private Color32 cardUpColor;
        [SerializeField] private Color32 cardDownColor;
        [SerializeField] private List<Color32> matchableColors;
        [SerializeField] private float cardRevealTimeoutSeconds = 1.0f;
        [SerializeField] private float initialRevealDuratoin = .5f;

        public List<Vector2Int> AvailableBoardSizes => availableBoardSizes;
        public CardVisual CardVisualPrefab => cardVisualPrefab;
        public Color32 CardUpColor => cardUpColor;
        public Color32 CardDownColor => cardDownColor;
        public List<Color32> MatchableColors => matchableColors;
        public float CardRevealTimeoutSeconds => cardRevealTimeoutSeconds;
        public float InitialRevealDuratoin => initialRevealDuratoin;
    }
}
