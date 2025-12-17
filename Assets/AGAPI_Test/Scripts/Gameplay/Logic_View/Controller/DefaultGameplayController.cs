using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGAPI.Foundation;

namespace AGAPI.Gameplay
{
    public class DefaultGameplayController : IGameplayController, IBoardInputHandler, IUIInputHandler
    {

        private IBoardManager _boardManager;
        private LevelProgression _levelProgression;

        private readonly IBoardVisuals _boardVisuals;
        private readonly BoardConfig _boardConfig;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameplayEvents _gameplayEvents;

        public DefaultGameplayController(IBoardVisuals boardVisuals, BoardConfig boardConfig, ICoroutineRunner coroutineRunner, GameplayEvents gameplayEvents)
        {
            _boardVisuals = boardVisuals;
            _boardConfig = boardConfig;
            _coroutineRunner = coroutineRunner;
            _gameplayEvents = gameplayEvents;
            Initialize();
            Debug.Log("DefaultGameplayController initialized.");
        }

        // ------- IGameplayContreoller iplementation -------
        public void ReportMatchResult(CardData firstCard, CardData secondCard, bool isMatch, int remainingPairs)
        {
            // to do : report result to score manager and fetch updated score
            // to do : update score on level progression
            // to do : trigger score update event
            _levelProgression.UpdateCardRecord(firstCard);
            _levelProgression.UpdateCardRecord(secondCard);
            if (remainingPairs == 0)
            {
                _levelProgression.MarkLevelkCompleted();
                _gameplayEvents.Invoke(new GameplayEvents.OnLevelCompleted());
            }
            // to do : set _levelProgression updates dirty on saving system
        }

        // ------- IBoardInputHandler iplementation -------
        public void HandleCardPick(int cardIndex)
        {
            _boardManager.PickCard(cardIndex);
        }

        // ------- IUIInputHandler iplementation -------
        public void StartNewGame(Vector2Int boardSize)
        {
            if (_boardManager.TryCreateNewBoard(boardSize, out var cardRecords, out var errorMessage))
            {
                _levelProgression.SetInitialCardRecords(new Dictionary<int, CardData>());
                _levelProgression.SetBoardSize(boardSize);
                _levelProgression.MarkLevelStarted();
                // to do : set _levelProgression updates dirty on saving system
                StartGame();

            }
            else
            {
                Debug.LogError($"Failed to start new game. Error: {errorMessage}");
            }
        }
        public bool TryContinueGame()
        {
            if (_levelProgression.IsLevelInProgress)
            {
                var boardSize = _levelProgression.BoardSize;
                var cardRecords = _levelProgression.GetCardRecords();
                _boardManager.CreateBoardFromRecord(boardSize, cardRecords);
                StartGame();
                return true;
            }
            return false;
        }
        public void ExitLevel()
        {
            //restart scene for now
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }


        // ------- Private methodes -------
        private void Initialize()
        {
            _levelProgression = new LevelProgression();
            _boardManager = new DefaultBoardManager(_boardVisuals, _boardConfig, this, _coroutineRunner);
        }

        void StartGame()
        {
            _gameplayEvents.Invoke(new GameplayEvents.OnLevelStarts());
            _boardManager.OnGameStart();
        }
    }
}

