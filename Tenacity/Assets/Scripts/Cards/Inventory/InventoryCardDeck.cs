using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tenacity.Cards.Inventory
{
    public class InventoryCardDeck : MonoBehaviour
    {
        [SerializeField] private CardDecksSO _cardDecks;
        [SerializeField] private GameObject _content;
        [SerializeField] private RectTransform _cardDeckButtonsArea;
        [Space]
        [SerializeField] private GameObject _cardDeckLine;
        [SerializeField] private GameObject _cardDeckButton;

        private CardDataSO _selectedCard;
        private List<Card> _cardDeckLines = new();
        private CardDeck _cardDeck => _cardDecks.CurrentlySelectedCardDeck;


        private void Awake()
        {
            if (_cardDeck == null) return;

            InitDeckButtons();
            CreateCardObjectsInCardDeck();

        }

        private void InitDeckButtons()
        {
            for (int i = 0; i < _cardDecks.MaxCardDeckQuantity; i++)
            {
                if (i >= _cardDecks.CurrentQuantity) _cardDecks.AddCardDeck(new CardDeck());
                var deckButton = Instantiate(_cardDeckButton, _cardDeckButtonsArea);

                deckButton.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
                int id = i;
                deckButton.GetComponent<Button>().onClick.AddListener(delegate {
                    var isSelected = _cardDecks.SelectCardDeck(id);
                    if (!isSelected) return;
                    CreateCardObjectsInCardDeck();
                });
            }
        }

        private void CreateCardObjectsInCardDeck()
        {
            ClearCardDeckArea();
            for (int i = 0; i < _cardDeck.Capacity; i++)
            {
                var cardDeckLine = CreateCardDeckLine();

                Card lineCardObject = cardDeckLine.GetComponent<Card>();
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

        private GameObject CreateCardDeckLine()
        {
            var cardDeckLine = Instantiate(_cardDeckLine, _content.transform);

            EventTrigger trigger = cardDeckLine.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new()
            {
                eventID = EventTriggerType.PointerClick
            };
            entry.callback.AddListener((data) => { RemoveCardFromCardDeck(cardDeckLine.GetComponent<Card>()); });
            trigger.triggers.Add(entry);

            return cardDeckLine;
        }

        /*
        private void DrawCardDeck()
        {
            ClearCardDeckArea();
            for (int i = 0; i < _cardDeck.Capacity; i++)
            {
                if (_content.childCount < i) CreateCardDeckLine();

                Card lineCardObject = cardDeckLine.GetComponent<Card>();
                if (i < _cardDeck.Cards.Count)
                {
                    lineCardObject.Data = _cardDeck.Cards[i];
                    cardDeckLine.GetComponent<CardDataDisplay>().DisplayCardValues();
                }
                else
                {
                    cardDeckLine.SetActive(false);
                }
            }
        }
        */

        private void ClearCardDeckArea()
        {
            if (_content.GetComponentsInChildren<Card>().Length == 0) return;
            foreach (Transform child in _content.transform)
            {
                Destroy(child.gameObject);
            }
            _cardDeckLines.Clear();
        }

        public void AddCardIntoCardDeck(CardDataSO cardData)
        {
            if (cardData == null || _cardDecks.CurrentlySelectedCardDeck == null) return;
            if (_selectedCard == null || _selectedCard != cardData)
            {
                _selectedCard = cardData;
                return;
            }
            _selectedCard = null;

            var index = _cardDecks.CurrentlySelectedCardDeck.AddCardData(cardData);
            if (index < 0) return;

            var line = _cardDeckLines[index];
            line.Data = cardData;
            line.GetComponent<CardDataDisplay>().DisplayCardValues();
            line.gameObject.SetActive(true);

        }

        private void RemoveCardFromCardDeck(Card card)
        {
            if (!_cardDeckLines.Contains(card)) return;
            if (_selectedCard == null || _selectedCard != card.Data)
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
    }
}