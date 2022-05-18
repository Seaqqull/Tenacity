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
        [SerializeField] private List<CardDataSO> _cards = new();

        public int Capacity => _capacity;
        public List<CardDataSO> Cards => _cards;

        public int AddCardData(CardDataSO cardData)
        {
            if (_cards.Count == _capacity) return -1;
            if (_cards.Contains(cardData)) return -1;
            _cards.Add(cardData);
            return _cards.Count-1;
        }

        public bool RemoveCardFromCardDeck(CardDataSO cardData)
        {
            return _cards.Remove(cardData);
        }
    }
}