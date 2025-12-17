using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace AGAPI.Gameplay
{
    public class LevelProgression
    {
        private LevelProgressionRecord _record = new();

        public bool IsLevelInProgress => _record.InProgress;
        public Vector2Int BoardSize => _record.BoardSize;

        public void SetInitialCardRecords(Dictionary<int, CardData> cardDatasByIndex)
        {
            // extra safety
            _record = new();

            foreach (var kvp in cardDatasByIndex)
            {
                UpdateCardRecord(kvp.Value);
            }
        }
        public Dictionary<int, CardRecord> GetCardRecords()
        {
            return _record.CardRecordsByIndex;
        }
        public void UpdateScore(int score)
        {
            _record.Score = score;
        }
        public void SetBoardSize(Vector2Int size)
        {
            _record.BoardSize = size;
        }
        public void MarkLevelkCompleted()
        {
            _record.InProgress = false;
        }
        public void MarkLevelStarted()
        {
            _record.InProgress = true;
        }
        public void UpdateCardRecord(CardData cardData)
        {
            var cardRecord = cardData.GetCardRecord();
            var index = cardRecord.CardIndex;

            if (_record.CardRecordsByIndex.ContainsKey(index))
            {
                _record.CardRecordsByIndex[index] = cardRecord;
            }
            else
            {
                _record.CardRecordsByIndex.Add(index, cardRecord);
            }
        }
    }
}
