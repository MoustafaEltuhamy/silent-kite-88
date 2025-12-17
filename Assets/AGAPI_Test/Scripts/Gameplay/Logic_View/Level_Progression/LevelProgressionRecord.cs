using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AGAPI.Gameplay
{
    public class LevelProgressionRecord
    {
        public bool InProgress = false;
        public int Score = 0;
        public Vector2Int BoardSize = new();
        public Dictionary<int, CardRecord> CardRecordsByIndex = new();
    }

}