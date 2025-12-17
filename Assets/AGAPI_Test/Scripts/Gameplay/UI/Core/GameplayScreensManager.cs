using System;
using System.Diagnostics;


namespace AGAPI.Gameplay
{
    public class GameplayScreensManager : UIScreenManager
    {
        protected override void OnConfigureScreen()
        {
            SubescripeToEvents();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnsubescripeFromEvents();
        }


        private void SubescripeToEvents()
        {
            GameplayEvents.Subscribe<GameplayEvents.OnLevelStarts>(OnLevelStarts);
            GameplayEvents.Subscribe<GameplayEvents.OnLevelCompleted>(OnLevelCompleted);
        }

        private void UnsubescripeFromEvents()
        {
            GameplayEvents.Unsubscribe<GameplayEvents.OnLevelStarts>(OnLevelStarts);
            GameplayEvents.Unsubscribe<GameplayEvents.OnLevelCompleted>(OnLevelCompleted);
        }

        private void OnLevelCompleted(GameplayEvents.OnLevelCompleted completed)
        {
            ScreenFocus<LevelCompleteScreen>();
        }

        private void OnLevelStarts(GameplayEvents.OnLevelStarts starts)
        {
            ScreenFocus<GameplayScreen>();
        }
    }
}
