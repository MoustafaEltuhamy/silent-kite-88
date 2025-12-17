using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Gameplay
{
    public interface IBoardInputHandler
    {
        void HandleCardPick(int cardIndex);
    }
}
