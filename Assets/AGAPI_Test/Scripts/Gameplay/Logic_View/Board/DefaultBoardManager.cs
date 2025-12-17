using System;
using System.Collections.Generic;
using UnityEngine;
using AGAPI.Foundation;
using System.Collections;

namespace AGAPI.Gameplay
{
    public class DefaultBoardManager : IBoardManager
    {
        private int _remainingPairs = 0;
        private Vector2Int _boardSize;
        private Queue<CardData> _pickedCards = new();

        private readonly Dictionary<int, CardData> _cardsByIndex = new();
        private readonly IBoardVisuals _boardVisuals;
        private readonly BoardConfig _boardConfig;
        private readonly IGameplayController _gameplayController;
        private readonly ICoroutineRunner _coroutineRunner;

        public DefaultBoardManager(IBoardVisuals boardVisuals, BoardConfig config, IGameplayController controller, ICoroutineRunner coroutineRunner)
        {
            _boardVisuals = boardVisuals;
            _boardConfig = config;
            _gameplayController = controller;
            _coroutineRunner = coroutineRunner;
            Debug.Log("DefaultBoardManager initialized.");
        }


        // ------------- IBoardManager implementation -------------
        public bool TryCreateNewBoard(Vector2Int size, out Dictionary<int, CardRecord> cardRecords, out string errorMessage)
        {
            errorMessage = string.Empty;
            cardRecords = null;

            // Validate input
            if (_boardConfig == null)
            {
                errorMessage = "BoardConfig is null. Cannot create board.";
                return false;
            }

            var availableIds = _boardConfig.MatchableColors;
            if (availableIds == null || availableIds.Count == 0)
            {
                errorMessage = "BoardConfig.MatchableColors is empty. Cannot create board.";
                return false;
            }

            if (size.x <= 0 || size.y <= 0)
            {
                errorMessage = $"Invalid board size {size}. Rows/Cols must be > 0.";
                return false;
            }

            int totalCards = checked(size.x * size.y);
            if ((totalCards & 1) == 1)
            {
                errorMessage = $"Board size {size} has odd total cards ({totalCards}). Must be even.";
                return false;
            }

            _boardSize = size;
            _cardsByIndex.Clear();
            int pairsNeeded = totalCards / 2;
            int idsCount = availableIds.Count;

            List<int> pairIds = BuildPairIdSequence(pairsNeeded, idsCount);

            // Expand pairs into card ids (two cards per pair)
            var cardIds = new List<int>(totalCards);
            for (int i = 0; i < pairIds.Count; i++)
            {
                int id = pairIds[i];
                cardIds.Add(id);
                cardIds.Add(id);
            }

            cardIds.Shuffle();

            for (int index = 0; index < totalCards; index++)
            {
                int cardId = cardIds[index];
                _cardsByIndex.Add(index, new CardData(cardId, index));
            }

            // initialize board visuals
            InitializeBoardVisuals();
            _remainingPairs = totalCards / 2;
            return true;
        }

        public void CreateBoardFromRecord(Vector2Int boardSize, Dictionary<int, CardRecord> cardRecords)
        {
            _boardSize = boardSize;
            _cardsByIndex.Clear();

            foreach (var kvp in cardRecords)
            {
                var cardData = new CardData(kvp.Value);
                _cardsByIndex.Add(kvp.Key, cardData);
            }

            // initialize board visuals
            InitializeBoardVisuals();
            var totalCards = cardRecords.Count;
            _remainingPairs = totalCards / 2;
        }

        public void PickCard(int cardIndex)
        {
            if (_cardsByIndex.TryGetValue(cardIndex, out var cardData))
            {
                HandleCardPick(cardData);
            }
            else
            {
                Debug.LogError($"PickCard: No card found at index {cardIndex}.");
            }
        }

        // ---------------- private methodes ----------------
        private void HandleCardPick(CardData pickedCard)
        {
            // Validate Pick
            if (pickedCard.IsMatched)
            {
                Debug.LogWarning($"HandleCardPick: Card at index {pickedCard.Index} is already matched.");
                return;
            }
            if (pickedCard.CardState == CardState.Up)
            {
                Debug.LogWarning($"HandleCardPick: Card at index {pickedCard.Index} is already face up.");
                return;
            }
            if (pickedCard.CardState == CardState.Locked)
            {
                Debug.LogWarning($"HandleCardPick: Card at index {pickedCard.Index} is locked.");
                return;
            }

            // Process Pick
            ProcessPickedCard(pickedCard);
        }

        void ProcessPickedCard(CardData pickedCard)
        {
            _boardVisuals.FlipCardUp(pickedCard.Index, () => OnCardFlipUpComplete(pickedCard));
            pickedCard.SetCardState(CardState.Locked);
            _pickedCards.Enqueue(pickedCard);
            if (_pickedCards.Count >= 2)
            {
                var firstCard = _pickedCards.Dequeue();
                var secondCard = _pickedCards.Dequeue();
                _coroutineRunner.Run(ResolvePairWhenBothUp(firstCard, secondCard));
            }
        }

        private IEnumerator ResolvePairWhenBothUp(CardData firstCard, CardData secondCard)
        {
            // Safety: avoid infinite wait if something goes wrong.
            float timeoutSeconds = _boardConfig.CardRevealTimeoutSeconds;
            float elapsed = 0f;

            while (!(firstCard.CardState == CardState.Up && secondCard.CardState == CardState.Up))
            {
                elapsed += Time.deltaTime;
                if (elapsed >= timeoutSeconds)
                {
                    Debug.LogWarning(
                        $"ResolvePairWhenBothUp timed out. First={firstCard.Index}:{firstCard.CardState}, Second={secondCard.Index}:{secondCard.CardState}");
                    yield break;
                }

                yield return null;
            }

            ComparePickedPair(firstCard, secondCard);
        }

        private void ComparePickedPair(CardData firstCard, CardData secondCard)
        {
            bool isMatch = firstCard.CardId == secondCard.CardId;

            if (isMatch)
            {
                firstCard.SetMatched(true);
                secondCard.SetMatched(true);
                _boardVisuals.MatchCard(firstCard.Index);
                _boardVisuals.MatchCard(secondCard.Index);

                _remainingPairs--;
            }
            else
            {
                _boardVisuals.FlipCardDown(firstCard.Index, () => OnCardFlipDownComplete(firstCard));
                _boardVisuals.FlipCardDown(secondCard.Index, () => OnCardFlipDownComplete(secondCard));
            }

            // Report result to controller
            _gameplayController.ReportMatchResult(firstCard, secondCard, isMatch, _remainingPairs);
        }
        private void OnCardFlipUpComplete(CardData cardData)
        {
            cardData.SetCardState(CardState.Up);
        }
        private void OnCardFlipDownComplete(CardData cardData)
        {
            cardData.SetCardState(CardState.Down);
        }

        private List<int> BuildPairIdSequence(int pairsNeeded, int idsCount)
        {
            var result = new List<int>(pairsNeeded);

            var pool = new List<int>(idsCount);
            for (int i = 0; i < idsCount; i++) pool.Add(i);

            // If we need fewer pairs than ids available.
            if (pairsNeeded <= idsCount)
            {
                pool.Shuffle();
                for (int i = 0; i < pairsNeeded; i++)
                    result.Add(pool[i]);

                return result;
            }

            // Otherwise, repeatedly consume full shuffled pools.
            int remaining = pairsNeeded;
            while (remaining > 0)
            {
                pool.Shuffle();

                int take = Mathf.Min(remaining, idsCount);
                for (int i = 0; i < take; i++)
                    result.Add(pool[i]);

                remaining -= take;
            }

            return result;
        }

        private void InitializeBoardVisuals()
        {
            _boardVisuals.InitializeVisuals(_boardSize, _cardsByIndex);
        }
    }
}
