using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AGAPI.Gameplay
{
    public abstract class UIScreenManager : MonoBehaviour
    {

        [SerializeField]
        protected UIScreenBehaviour DefaultScreen;


        // Dictionary to cache all UIScreenBehaviour instances by their Type.
        protected readonly Dictionary<Type, UIScreenBehaviour> UIScreens = new Dictionary<Type, UIScreenBehaviour>();

        protected UIScreenBehaviour CurrentScreen;

        // Set to track active overlay screens.
        private readonly HashSet<UIScreenBehaviour> activeOverlays = new HashSet<UIScreenBehaviour>();


        public RectTransform RectTransform { get; private set; }
        public Camera CanvasCamera { get; private set; }
        public IUIInputHandler UiInputHandler { get; private set; }
        public GameplayEvents GameplayEvents { get; private set; }
        public BoardConfig BoardConfig { get; private set; }

        protected virtual void Awake()
        {
            InitializeComponents();
            CacheUIScreenBehaviours();
            InitializeScreens();
        }

        public void ConfigureScreen(IUIInputHandler handler, GameplayEvents gameplayEvents, BoardConfig boardConfig)
        {
            GameplayEvents = gameplayEvents;
            UiInputHandler = handler;
            BoardConfig = boardConfig;
            OnConfigureScreen();
            // Notify all screens that services are ready
            foreach (var screen in UIScreens.Values)
                screen.OnConfigured();

            ActivateDefaultScreen();
        }

        public void ScreenFocus<T>(bool closeAllOverlays = false) where T : UIScreenBehaviour
        {
            var type = typeof(T);

            if (!UIScreens.TryGetValue(type, out var targetScreen))
            {
                Debug.LogError($"Screen of type {type} not found.");
                return;
            }

            if (CurrentScreen == targetScreen)
            {
                Debug.LogWarning("Attempting to focus on the currently active screen.");
                return;
            }

            // Unfocus the current screen and focus the target screen.
            CurrentScreen?.Unfocus();
            CurrentScreen = targetScreen;
            CurrentScreen.Focus();
        }

        protected virtual void OnConfigureScreen() { }

        protected virtual void OnDestroy()
        {
            UninstallScreens();
        }

        protected virtual void CacheUIScreenBehaviours()
        {
            foreach (var screen in GetComponentsInChildren<UIScreenBehaviour>(true))
            {
                var type = screen.GetType();
                if (!UIScreens.TryAdd(type, screen))
                {
                    Debug.LogError(
                        $"Duplicate UIScreenBehaviour of type {type} found on '{screen.gameObject.name}'. Only the first instance will be used.");
                }
            }
        }

        protected virtual void InitializeScreens()
        {
            foreach (var screen in UIScreens.Values)
            {
                screen.Initialize(this);
                screen.Unfocus();
            }
        }

        protected virtual void UninstallScreens()
        {
            foreach (var screen in UIScreens.Values)
            {
                screen.Uninstall();
            }
        }



        private void InitializeComponents()
        {
            RectTransform = GetComponent<RectTransform>();
            // Cache the camera used by the canvas.
            Canvas canvas = GetComponent<Canvas>();
            var camera = canvas != null ? canvas.worldCamera : null;
            CanvasCamera = camera != null ? camera : Camera.main;
        }
        private void ActivateDefaultScreen()
        {
            if (DefaultScreen != null)
            {
                DefaultScreen.Focus();
                CurrentScreen = DefaultScreen;
            }
            else
            {
                Debug.LogWarning("Default screen is not set in the UI Canvas Manager.");
            }
        }


    }
}
