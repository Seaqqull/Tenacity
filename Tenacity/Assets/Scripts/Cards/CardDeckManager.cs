using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tenacity.PlayerInventory;
using UnityEngine;

namespace Tenacity.Cards
{
    public class CardDeckManager : MonoBehaviour
    {
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private Transform[] _cardPositions;
        [SerializeField] private InventoryData _inventoryCards;
        [SerializeField] private List<CardData> _cardsDataPack = new List<CardData>();


        private List<Card> _cardPack = new List<Card>();


        public List<Card> CardPack => _cardPack;

        private void Awake()
        {
            if (_inventoryCards == null || _cardPositions == null) return;

            _cardsDataPack = GetRandomCardsFromLInventory(_inventoryCards.Cards, _cardPositions.Length);
            InitCardDeck(_cardsDataPack);
        }

        private void InitCardDeck(List<CardData> cardDataPack)
        {
            if (cardDataPack.Count <= 0) return;

            List<CardData> copyPack = new List<CardData>(cardDataPack);
            for (int i = 0; i < _cardPositions.Length; i++)
            {
                var cardData = copyPack[Random.Range(0, copyPack.Count)];
                var createdCard = AddCardToDeck(cardData, i);
                copyPack.Remove(cardData);
                _cardPack.Add(createdCard);
            }
        }

        private Card AddCardToDeck(CardData cardData, int slotId)
        {
            Card card = Instantiate(_cardPrefab, _cardPositions[slotId]);
            card.GetComponent<RectTransform>().SetParent(_cardPositions[slotId]);
            card.Data = cardData;
            card.gameObject.SetActive(true);
            return card;
        }

        private List<CardData> GetRandomCardsFromLInventory(List<CardData> list, int number)
        {
            List<CardData> tmpList = new List<CardData>(list);
            List<CardData> newList = new List<CardData>();

            while (newList.Count < number && tmpList.Count > 0)
            {
                int id = Random.Range(0, tmpList.Count);
                newList.Add(tmpList[id]);
                tmpList.RemoveAt(id);
            }
            return newList;
        }

    }
}