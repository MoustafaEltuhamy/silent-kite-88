using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Gameplay
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private BoardConfig boardConfig;

        [Header("Scene Refs")]
        [SerializeField] private DefaultBoardVisuals boardVisuals;
        [SerializeField] private UIScreenManager screensManager;

        private ICoroutineRunner _coroutineRunner;

        void Start()
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
            screensManager.ConfigureScreen(gameController, gameplayEvents, boardConfig);

        }

    }
}