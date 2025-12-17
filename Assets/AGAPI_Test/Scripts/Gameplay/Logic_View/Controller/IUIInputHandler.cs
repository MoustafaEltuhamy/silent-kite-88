using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AGAPI.Gameplay
{
    public interface IUIInputHandler
    {
        void StartNewGame(Vector2Int boardSize);
        bool TryContinueGame();
        void ExitLevel();
    }
}
