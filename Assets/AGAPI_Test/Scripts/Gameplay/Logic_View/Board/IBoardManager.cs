using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AGAPI.Gameplay
{
    public interface IBoardManager
    {
        bool TryCreateNewBoard(Vector2Int size, out Dictionary<int, CardRecord> cardRecords, out string errorMessage);
        void CreateBoardFromRecord(Vector2Int boardSize, Dictionary<int, CardRecord> cardRecords);
        void PickCard(int cardIndex);

    }
}
