using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AGAPI.Gameplay
{
    public abstract class UIScreenBehaviour : MonoBehaviour
    {
        protected UIScreenManager ScreenManager;

        public virtual void Initialize(UIScreenManager screenManager)
        {
            ScreenManager = screenManager;
        }

        public virtual void OnConfigured() { }

        public void Focus()
        {
            gameObject.SetActive(true);
            OnFocus();
        }

        public void Unfocus()
        {
            OnUnfocus();
            gameObject.SetActive(false);
        }

        //called when the canvas manager is destroyed
        public virtual void Uninstall()
        {
        }

        // Hooks for additional behavior during focus/unfocus
        protected virtual void OnFocus()
        {
        }
        protected virtual void OnUnfocus()
        {
        }

    }
}
