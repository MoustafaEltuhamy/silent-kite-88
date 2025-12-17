using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGAPI.Foundation;
using AGAPI.Systems;

namespace AGAPI.Gameplay
{
    public class DefaultGameplayController : IGameplayController, IBoardInputHandler, IUIInputHandler
    {
        // Aliases; 
        private static readonly Key FileKey = PresistanceKeys.Gameplay.FileKey;
        private static readonly Key LevelProgressionDataKey = PresistanceKeys.Gameplay.LevelProgressionDataKey;

        private IBoardManager _boardManager;
        private LevelProgression _levelProgression;
        private IScoreSystem _scoreSystem;

        private readonly IBoardVisuals _boardVisuals;
        private readonly BoardConfig _boardConfig;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameplayEvents _gameplayEvents;
        private readonly ScoreConfig _scoreConfig;
        private readonly IPersistenceService _persistenceService;

        public DefaultGameplayController(IBoardVisuals boardVisuals, BoardConfig boardConfig,
                                        ICoroutineRunner coroutineRunner, GameplayEvents gameplayEvents,
                                        ScoreConfig scoreConfig, IPersistenceService persistenceService)
        {
            _boardVisuals = boardVisuals;
            _boardConfig = boardConfig;
            _coroutineRunner = coroutineRunner;
            _gameplayEvents = gameplayEvents;
            _scoreConfig = scoreConfig;
            _persistenceService = persistenceService;

            Initialize();
            Debug.Log("DefaultGameplayController initialized.");
        }

        // ------- IGameplayContreoller iplementation -------
        public void ReportMatchResult(CardData firstCard, CardData secondCard, bool isMatch, int remainingPairs)
        {
            _scoreSystem.ApplyMatchResult(isMatch);
            _gameplayEvents.Invoke(new GameplayEvents.OnScoreUpdate(_scoreSystem.Score));

            _levelProgression.UpdateScore(_scoreSystem.Score);
            _levelProgression.UpdateCardRecord(firstCard);
            _levelProgression.UpdateCardRecord(secondCard);
            if (remainingPairs == 0)
            {
                _levelProgression.MarkLevelkCompleted();
                _gameplayEvents.Invoke(new GameplayEvents.OnLevelCompleted());
            }

            _persistenceService.MarkDirty(FileKey, LevelProgressionDataKey, _levelProgression);
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
                _scoreSystem.ResetScore();
                _levelProgression.ResetProgression();
                _levelProgression.SetInitialCardRecords(cardRecords);
                _levelProgression.SetBoardSize(boardSize);
                _levelProgression.MarkLevelStarted();
                _levelProgression.UpdateScore(_scoreSystem.Score);
                _persistenceService.MarkDirty(FileKey, LevelProgressionDataKey, _levelProgression);
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
                _scoreSystem.ResetScore(_levelProgression.Score);
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
            _levelProgression = (LevelProgression)_persistenceService.Load(FileKey, LevelProgressionDataKey, new LevelProgression());
            _boardManager = new DefaultBoardManager(_boardVisuals, _boardConfig, this, _coroutineRunner);
            _scoreSystem = new DefaultScoreSystem(_scoreConfig);
        }

        void StartGame()
        {
            _gameplayEvents.Invoke(new GameplayEvents.OnLevelStarts());
            _gameplayEvents.Invoke(new GameplayEvents.OnScoreUpdate(_scoreSystem.Score));
            _boardManager.OnGameStart();
        }
    }
}

