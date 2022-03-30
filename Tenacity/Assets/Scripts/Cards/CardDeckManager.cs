using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tenacity.PlayerInventory;
using UnityEngine;

namespace Tenacity.Cards
{
    public class CardDeckManager : MonoBehaviour
    {
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform[] cardPositions;
        [SerializeField] private InventoryData inventoryCards;
        [SerializeField] private List<CardData> cardsDataPack = new List<CardData>();

        private List<Card> _cardPack = new List<Card>();

        public List<Card> CardPack => _cardPack;

        private void Awake()
        {
            if (inventoryCards == null || cardPositions == null) return;

            cardsDataPack = GetRandomCardsFromList(inventoryCards.Cards, cardPositions.Length);
            DrawCardPack(cardsDataPack);
        }

        private List<CardData> GetRandomCardsFromList(List<CardData> list, int number)
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

        private void DrawCardPack(List<CardData> pack)
        {
            if (pack.Count <= 0) return;

            List<CardData> copyPack = new List<CardData>(pack);
            for (int i = 0; i < cardPositions.Length; i++)
            {
                var cardData = copyPack[Random.Range(0, copyPack.Count)];
                var createdCard = CreateCardOnDeck(cardData, i);
                copyPack.Remove(cardData);
                _cardPack.Add(createdCard);
            }
        }

        private Card CreateCardOnDeck(CardData cardData, int slotId)
        {
            Card card = Instantiate(cardPrefab, cardPositions[slotId]);
            card.transform.parent = cardPositions[slotId];
            card.Data = cardData;
            card.gameObject.SetActive(true);
            return card;
        }

        //tmp
        public bool ReplaceCard(Card replaceableCard, Card card)
        {
            int id = _cardPack.FindIndex(el => el == replaceableCard);
            if (id != -1)
            {
                _cardPack[id] = card;
                return true;
            }
            return false;
        }
    }
}