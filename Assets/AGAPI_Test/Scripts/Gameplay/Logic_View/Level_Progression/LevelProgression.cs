using System.Collections;
using System.Collections.Generic;
using AGAPI.Systems;
using UnityEngine;


namespace AGAPI.Gameplay
{
    public class LevelProgression : IPersistable<LevelProgressionRecord>
    {
        private LevelProgressionRecord _record = new();

        public bool IsLevelInProgress => _record.InProgress;
        public Vector2Int BoardSize => _record.BoardSize;
        public int Score => _record.Score;

        public void ResetProgression()
        {
            _record = new LevelProgressionRecord();
        }
        public void SetInitialCardRecords(Dictionary<int, CardData> cardDatasByIndex)
        {
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


        // ------- IPersistable implementation -------
        public LevelProgressionRecord GetRecordSnapshot()
        {
            return _record;
        }

        public void LoadRecord(LevelProgressionRecord record)
        {
            if (record == null)
            {
                return;
            }

            _record = record;
        }
    }
}
