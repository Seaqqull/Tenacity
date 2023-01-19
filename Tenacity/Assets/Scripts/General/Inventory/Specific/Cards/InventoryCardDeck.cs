using System.Collections.Generic;
using Tenacity.Cards.Cards.Data;
using UnityEngine.EventSystems;
using Tenacity.UI.Cards;
using UnityEngine;


namespace Tenacity.Cards.Inventory
{
    public class InventoryCardDeck : MonoBehaviour
    {
        [SerializeField] private CardDeckSO _cardDeck;
        [SerializeField] private GameObject _content;
        [SerializeField] private RectTransform _cardDeckButtonsArea;
        [Space]
        [SerializeField] private GameObject _cardDeckLine;
        [SerializeField] private DeckSwitcher _cardDeckButton;

        private List<CardItem> _cardDeckLines = new();
        private DeckSwitcher _selectedSwitcher;
        private DeckSwitcher _switcher;
        private CardSO _selectedCard;
        

        private void Awake()
        {
            if (_cardDeck == null) return;
            _cardDeck.Cards.Clear();

            InitDeckButtons();
            CreateCardObjectsInCardDeck();
            
            _switcher.Switch();
        }

        
        private void InitDeckButtons()
        {
            _switcher = Instantiate(_cardDeckButton, _cardDeckButtonsArea);

            _switcher.Text = "1";
            _switcher.OnClickAction += (switcher) => { };
        }
        
        private void ClearCardDeckArea()
        {
            if (_content.GetComponentsInChildren<CardItem>().Length == 0) return;
            foreach (Transform child in _content.transform)
                Destroy(child.gameObject);

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
                RemoveCardFromCardDeck(cardDeckLine.GetComponent<CardItem>());
            });
            trigger.triggers.Add(entry);

            return cardDeckLine;
        }
        
        private void CreateCardObjectsInCardDeck()
        {
            ClearCardDeckArea();
            
            for (int i = 0; i < _cardDeck.Capacity; i++)
            {
                var cardDeckLine = CreateCardDeckLine();
                var lineCardObject = cardDeckLine.GetComponent<CardItem>();
                
                if (i < _cardDeck.Cards.Count)
                {
                    lineCardObject.Data = _cardDeck.Cards[i];
                    cardDeckLine.GetComponent<CardDataDisplay>().DisplayCardValues();
                }
                else
                {
                    cardDeckLine.SetActive(false);
                }
                _cardDeckLines.Add(lineCardObject);
            }
        }
        
        private void RemoveCardFromCardDeck(CardItem card)
        {
            if (!_cardDeckLines.Contains(card)) return;
            if ((_selectedCard == null) || (_selectedCard != card.Data))
            {
                _selectedCard = card.Data;
                return;
            }
            _selectedCard = null;
            _cardDeck.RemoveCardFromCardDeck(card.Data);

            var line = _cardDeckLines.Find(c => c == card);
            line.Data = null;
            line.gameObject.SetActive(false);
            line.transform.SetAsLastSibling();
        }
        
        
        public void AddCardIntoCardDeck(CardSO cardData)
        {
            if ((cardData == null) || (_cardDeck == null)) return;
            if ((_selectedCard == null) || (_selectedCard != cardData))
            {
                _selectedCard = cardData;
                return;
            }
            
            _selectedCard = null;
            _cardDeck.AddCardData(cardData);
            
            ClearCardDeckArea();
            CreateCardObjectsInCardDeck();
        }
    }
}