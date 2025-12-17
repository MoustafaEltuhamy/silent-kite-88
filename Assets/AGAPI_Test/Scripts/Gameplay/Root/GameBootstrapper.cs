using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Gameplay
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private BoardConfig boardConfig;

        [Header("Refs")]
        [SerializeField] private DefaultBoardVisuals boardVisuals;

        private ICoroutineRunner _coroutineRunner;

        void Awake()
        {
            InsureRefrances();
            InstanciatePrisitanceObjects();
            InstanciateGameplaySystems();
        }


        void InsureRefrances()
        {
            if (TryGetComponent<ICoroutineRunner>(out var coroutineRunner))
            {
                _coroutineRunner = coroutineRunner;
            }
            else
            {
                _coroutineRunner = gameObject.AddComponent<DefaultCoroutineRunner>();
            }
        }

        void InstanciatePrisitanceObjects()
        {

        }

        void InstanciateGameplaySystems()
        {
            var gameplayEvents = new GameplayEvents();
            var gameController = new DefaultGameplayController(boardVisuals, boardConfig, _coroutineRunner, gameplayEvents);
            boardVisuals.Initialize(boardConfig, gameController);


            // to do : Remove this, game should start from menu ui not here!
            TempStartGame(gameController);
        }

        void TempStartGame(IUIInputHandler uIInputHandler)
        {
            uIInputHandler.StartNewGame(new Vector2Int(5, 4));
        }
    }
}