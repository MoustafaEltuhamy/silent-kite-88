using System.Collections.Generic;
using AGAPI.Systems;
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
        [SerializeField] private SoundData cardFlippingSound;
        [SerializeField] private SoundData cardMatchSound;
        [SerializeField] private SoundData cardMissmatchSound;
        [SerializeField] private SoundData gameOverSound;



        public List<Vector2Int> AvailableBoardSizes => availableBoardSizes;
        public CardVisual CardVisualPrefab => cardVisualPrefab;
        public Color32 CardUpColor => cardUpColor;
        public Color32 CardDownColor => cardDownColor;
        public List<Color32> MatchableColors => matchableColors;
        public float CardRevealTimeoutSeconds => cardRevealTimeoutSeconds;
        public float InitialRevealDuratoin => initialRevealDuratoin;
        public SoundData CardFlippingSound => cardFlippingSound;
        public SoundData CardMatchSound => cardMatchSound;
        public SoundData CardMissmatchSound => cardMissmatchSound;
        public SoundData GameOverSound => gameOverSound;

    }
}
