using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AGAPI.Gameplay
{
    public class CardData
    {
        private CardRecord _cardRecord;
        private CardState _cardState;

        public CardState CardState => _cardState;
        public int CardId => _cardRecord.CardID;
        public int Index => _cardRecord.CardIndex;
        public bool IsMatched => _cardRecord.Matched;

        public CardData(int cardId, int cardIndex)
        {
            _cardRecord = new CardRecord()
            {
                CardID = cardId,
                CardIndex = cardIndex,
                Matched = false
            };
            _cardState = CardState.Down;
        }
        public CardData(CardRecord cardRecord)
        {
            _cardRecord = cardRecord;
            _cardState = CardState.Down;
        }

        public void SetCardState(CardState newState)
        {
            _cardState = newState;
        }
        public void SetMatched(bool matched)
        {
            _cardRecord.Matched = matched;
        }
        public CardRecord GetCardRecord()
        {
            return _cardRecord;
        }
    }
}
