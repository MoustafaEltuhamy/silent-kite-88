using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AGAPI.Gameplay
{
    public interface IBoardManager
    {
        bool TryCreateNewBoard(Vector2Int size, out Dictionary<int, CardData> cardsByIndex, out string errorMessage);
        void CreateBoardFromRecord(Vector2Int boardSize, Dictionary<int, CardRecord> cardRecords);
        void OnGameStart();
        void PickCard(int cardIndex);

    }
}
