using System;
using System.Collections;
using System.Collections.Generic;
using AGAPI.Systems;
using UnityEngine;

namespace AGAPI.Gameplay
{
    public interface IBoardVisuals
    {
        void Initialize(BoardConfig boardConfig, IBoardInputHandler boardInputHandler, ISoundManager soundManager);
        void InitializeVisuals(Vector2Int boardSize, Dictionary<int, CardData> cardsByIndex);
        void FlipCardUp(int cardIndex, Action onFlipComplete = null);
        void FlipCardDown(int cardIndex, Action onFlipComplete = null);
        void MatchCard(int cardIndex);
    }
}
