using System;
using System.Collections;
using System.Collections.Generic;
using AGAPI.Foundation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace AGAPI.Gameplay
{
    public class GameplayScreen : UIScreenBehaviour
    {
        [SerializeField] private Button giveUpButton;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private string ScorePrefix = "Score: ";


        private GameplayEvents GameplayEvents => ScreenManager.GameplayEvents;
        private IUIInputHandler UiInputHandler => ScreenManager.UiInputHandler;

        public override void OnConfigured()
        {
            SetupButtons();
            SubscribeToEvents();
        }

        public override void Uninstall()
        {
            base.Uninstall();

            UnsubscribeFromEvents();
        }


        //-----------private methode----------

        private void SetupButtons()
        {
            giveUpButton.onClick.AddListener(UiInputHandler.ExitLevel);
        }

        private void SubscribeToEvents()
        {
            GameplayEvents?.Subscribe<GameplayEvents.OnScoreUpdate>(OnScoreUpdate);
        }

        private void UnsubscribeFromEvents()
        {
            GameplayEvents?.Unsubscribe<GameplayEvents.OnScoreUpdate>(OnScoreUpdate);
        }

        private void OnScoreUpdate(GameplayEvents.OnScoreUpdate update)
        {
            scoreText.text = ScorePrefix + update.NewScore.ToString();
        }
    }
}
