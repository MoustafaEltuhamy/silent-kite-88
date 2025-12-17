using System.Collections;
using System.Collections.Generic;
using AGAPI.Foundation;
using AGAPI.Systems;
using UnityEngine;

namespace AGAPI.Gameplay
{
    public class GameBootstrapper : MonoBehaviour
    {
        private static bool IsPrisitanceInstanciated = false;
        private static IPersistenceService PersistenceService;

        [Header("Configs")]
        [SerializeField] private BoardConfig boardConfig;
        [SerializeField] private ScoreConfig scoreConfig;

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
            if (IsPrisitanceInstanciated)
                return;

            var sessionEvents = new SessionEvents();

            var go = new GameObject("PersistentServices");
            DontDestroyOnLoad(go);

            var sessionService = go.AddComponent<SessionService>();
            sessionService.Configure(sessionEvents);

            var serializer = new NewtonsoftJsonStringSerializer();
            var envelopeHandler = new TextEnvelopeHandler(serializer);
            PersistenceService = new PersistenceService_StringEnv_StringPay(Application.persistentDataPath, "json", envelopeHandler, serializer, sessionEvents);

            IsPrisitanceInstanciated = true;
        }

        void InstanciateGameplaySystems()
        {
            var gameplayEvents = new GameplayEvents();
            var gameController = new DefaultGameplayController(boardVisuals, boardConfig, _coroutineRunner, gameplayEvents, scoreConfig, PersistenceService);

            boardVisuals.Initialize(boardConfig, gameController);
            screensManager.ConfigureScreen(gameController, gameplayEvents, boardConfig);

        }

    }
}