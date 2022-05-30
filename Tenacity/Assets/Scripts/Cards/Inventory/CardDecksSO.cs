using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tenacity.Cards.Inventory
{
    [CreateAssetMenu(fileName = "Card Decks Template", menuName = "CardDecks")]
    public class CardDecksSO : ScriptableObject
    {
        [SerializeField] private List<CardDeck> _cardDecks = new();
        [SerializeField] private int _maxCardDeckCount;

        private int _selectedDeckId;

        public int CurrentQuantity => _cardDecks.Count;
        public int MaxCardDeckQuantity => _maxCardDeckCount;


        public int CurrentlySelectedCardDeckId => _selectedDeckId;
        public CardDeck CurrentlySelectedCardDeck =>
            (_cardDecks.Count > _selectedDeckId)
            ? _cardDecks[_selectedDeckId]
            : null;

        public void AddCardDeck(CardDeck deck)
        {
            Debug.Log(_cardDecks.Count);
            if (CurrentQuantity == _maxCardDeckCount) return;
            _cardDecks.Add(deck);
        }

        public bool SelectCardDeck(int cardDeckId)
        {
            if (_selectedDeckId == cardDeckId) return false;
            _selectedDeckId = cardDeckId;
            return true;
        }

    }
}