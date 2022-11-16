using System.Collections;
using System.Collections.Generic;
using Tenacity.Cards;
using UnityEngine;

namespace Tenacity.Cards.Inventory
{
    [System.Serializable]
    public class CardDeck
    {
        [SerializeField] [Range(5, 10)] private int _capacity;
        [SerializeField] private List<CardSO> _cards = new();

        public int Capacity => _capacity;
        public List<CardSO> Cards => _cards;

        public int AddCardData(CardSO cardData)
        {
            if (_cards.Count == _capacity) return -1;
            if (_cards.Contains(cardData)) return -1;
            _cards.Add(cardData);
            return _cards.Count-1;
        }

        public bool RemoveCardFromCardDeck(CardSO cardData)
        {
            return _cards.Remove(cardData);
        }
    }
}