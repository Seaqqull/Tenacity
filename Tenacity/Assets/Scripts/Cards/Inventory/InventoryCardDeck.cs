using System.Collections.Generic;
using UnityEngine.EventSystems;
using Tenacity.UI.Cards;
using UnityEngine;


namespace Tenacity.Cards.Inventory
{
    public class InventoryCardDeck : MonoBehaviour
    {
        [SerializeField] private CardDecksSO _cardDecks;
        [SerializeField] private GameObject _content;
        [SerializeField] private RectTransform _cardDeckButtonsArea;
        [Space]
        [SerializeField] private GameObject _cardDeckLine;
        [SerializeField] private DeckSwitcher _cardDeckButton;

        private List<Card> _cardDeckLines = new();
        private DeckSwitcher _selectedSwitcher;
        private DeckSwitcher[] _switchers;
        private CardDataSO _selectedCard;
        
        private CardDeck CardDeck => _cardDecks.CurrentlySelectedCardDeck;


        private void Awake()
        {
            if (CardDeck == null) return;

            InitDeckButtons();
            CreateCardObjectsInCardDeck();
            _selectedSwitcher = _switchers[_cardDecks.CurrentlySelectedCardDeckId];
            _selectedSwitcher.Switch();
        }

        
        private void InitDeckButtons()
        {
            _switchers = new DeckSwitcher[_cardDecks.MaxCardDeckQuantity];
            for (int i = 0; i < _cardDecks.MaxCardDeckQuantity; i++)
            {
                if (i >= _cardDecks.CurrentQuantity) _cardDecks.AddCardDeck(new CardDeck());
                var deckButton = Instantiate(_cardDeckButton, _cardDeckButtonsArea);

                deckButton.Text = (i + 1).ToString();
                var id = i;
                deckButton.OnClickAction += (switcher) => {
                    var isSelected = _cardDecks.SelectCardDeck(id);
                    if (!isSelected) return;
                    
                    CreateCardObjectsInCardDeck();
                    _selectedSwitcher.Switch();
                    _selectedSwitcher = switcher;
                };
                _switchers[i] = deckButton;
            }
        }
        
        private void ClearCardDeckArea()
        {
            if (_content.GetComponentsInChildren<Card>().Length == 0) return;
            foreach (Transform child in _content.transform)
            {
                Destroy(child.gameObject);
            }
            _cardDeckLines.Clear();
        }
        
        private GameObject CreateCardDeckLine()
        {
            var cardDeckLine = Instantiate(_cardDeckLine, _content.transform);
            var trigger = cardDeckLine.GetComponent<EventTrigger>();
            
            EventTrigger.Entry entry = new() {
                eventID = EventTriggerType.PointerClick
            };
            entry.callback.AddListener((data) => {
                RemoveCardFromCardDeck(cardDeckLine.GetComponent<Card>());
            });
            trigger.triggers.Add(entry);

            return cardDeckLine;
        }
        
        private void CreateCardObjectsInCardDeck()
        {
            ClearCardDeckArea();
            
            for (int i = 0; i < CardDeck.Capacity; i++)
            {
                var cardDeckLine = CreateCardDeckLine();
                var lineCardObject = cardDeckLine.GetComponent<Card>();
                
                if (i < CardDeck.Cards.Count)
                {
                    lineCardObject.Data = CardDeck.Cards[i];
                    cardDeckLine.GetComponent<CardDataDisplay>().DisplayCardValues();
                }
                else
                {
                    cardDeckLine.SetActive(false);
                }
                _cardDeckLines.Add(lineCardObject);
            }
        }
        
        private void RemoveCardFromCardDeck(Card card)
        {
            if (!_cardDeckLines.Contains(card)) return;
            if ((_selectedCard == null) || (_selectedCard != card.Data))
            {
                _selectedCard = card.Data;
                return;
            }
            _selectedCard = null;
            _cardDecks.CurrentlySelectedCardDeck.RemoveCardFromCardDeck(card.Data);

            var line = _cardDeckLines.Find(c => c == card);
            line.Data = null;
            line.gameObject.SetActive(false);
            line.transform.SetAsLastSibling();
        }
        
        
        public void AddCardIntoCardDeck(CardDataSO cardData)
        {
            if ((cardData == null) || (_cardDecks.CurrentlySelectedCardDeck == null)) return;
            if ((_selectedCard == null) || (_selectedCard != cardData))
            {
                _selectedCard = cardData;
                return;
            }
            
            _selectedCard = null;
            _cardDecks.CurrentlySelectedCardDeck.AddCardData(cardData);
            
            ClearCardDeckArea();
            CreateCardObjectsInCardDeck();
        }
    }
}