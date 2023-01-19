using System.Collections.Generic;
using UnityEngine;


namespace Tenacity.Cards.Cards.Data
{
    [CreateAssetMenu(fileName = "CardDeck", menuName = "CardDeck")]
    public class CardDeckSO : ScriptableObject
    {
        [SerializeField] [Range(10, 10)] private int _capacity;
        [SerializeField] private List<CardSO> _cards = new();

        public List<CardSO> Cards => _cards;
        public int Capacity => _capacity;

        
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