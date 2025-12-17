using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AGAPI.Gameplay
{
    public class LevelCompleteScreen : UIScreenBehaviour
    {

        [SerializeField] private Button exitLevelButton;

        private IUIInputHandler UiInputHandler => ScreenManager.UiInputHandler;

        public override void OnConfigured()
        {
            SetupButtons();
        }

        //-----------private methode----------

        private void SetupButtons()
        {
            exitLevelButton.onClick.AddListener(UiInputHandler.ExitLevel);
        }

    }
}