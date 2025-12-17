using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Gameplay
{
    public class DefaultBoardVisuals : MonoBehaviour, IBoardVisuals
    {
        [SerializeField] private StretchGridLayoutGroup boardLayoutGroup;

        private BoardConfig _boardConfig;
        private IBoardInputHandler _boardInputHandler;

        private readonly Dictionary<int, CardVisual> _cardsByIndex = new();


        //------------------IBoardVisuals Implementation------------------//

        public void Initialize(BoardConfig boardConfig, IBoardInputHandler boardInputHandler)
        {
            _boardConfig = boardConfig;
            _boardInputHandler = boardInputHandler;
        }

        public void InitializeVisuals(Vector2Int boardSize, Dictionary<int, CardData> cardsByIndex)
        {
            var cardUpColor = _boardConfig.CardUpColor;
            var cardDownColor = _boardConfig.CardDownColor;
            boardLayoutGroup.SetGridSize(boardSize);

            foreach (var kvp in cardsByIndex)
            {
                var cardIndex = kvp.Key;
                var cardData = kvp.Value;
                var cardId = cardData.CardId;
                var matched = cardData.IsMatched;

                // create card visual
                var cardVisual = Instantiate(_boardConfig.CardVisualPrefab, boardLayoutGroup.transform);
                var matchableColor = _boardConfig.MatchableColors[cardId];
                var initialRevealDuratoin = _boardConfig.InitialRevealDuratoin;
                Action onCardClicked = () => { _boardInputHandler.HandleCardPick(cardIndex); };
                cardVisual.Config(matched, cardUpColor, cardDownColor, matchableColor, initialRevealDuratoin, onCardClicked);

                // store reference
                _cardsByIndex[cardIndex] = cardVisual;
            }
        }

        public void FlipCardUp(int cardIndex, Action onFlipComplete = null)
        {
            if (_cardsByIndex.TryGetValue(cardIndex, out var cardVisual))
            {
                cardVisual.PlayFlipUpAnimation(onFlipComplete);
            }
        }
        public void FlipCardDown(int cardIndex, Action onFlipComplete = null)
        {
            if (_cardsByIndex.TryGetValue(cardIndex, out var cardVisual))
            {
                cardVisual.PlayFlipDownAnimation(onFlipComplete);
            }
        }

        public void MatchCard(int cardIndex)
        {
            if (_cardsByIndex.TryGetValue(cardIndex, out var cardVisual))
            {
                cardVisual.PlayMatchAnimation();
            }
        }
    }
}
