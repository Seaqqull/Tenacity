using System.Collections.Generic;
using Tenacity.Cards.Inventory;
using UnityEngine;


namespace Tenacity.Cards
{
    public class CardDeckManager : MonoBehaviour
    {
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private Transform[] _cardPositions;
        [SerializeField] private InventoryData _inventoryCards;
        [SerializeField] private List<CardDataSO> _cardsDataPack = new List<CardDataSO>();
        
        public List<Card> CardPack { get; private set; } = new List<Card>();


        private void Awake()
        {
            if ((_inventoryCards == null) || (_cardPositions == null)) return;

            _cardsDataPack = GetRandomCardsFromLInventory(_inventoryCards.Cards, _cardPositions.Length);
            InitCardDeck(_cardsDataPack);
        }

        
        private void InitCardDeck(List<CardDataSO> cardDataPack)
        {
            if (cardDataPack.Count <= 0) return;

            var copyPack = new List<CardDataSO>(cardDataPack);
            for (int i = 0; i < _cardPositions.Length; i++)
            {
                var cardData = copyPack[Random.Range(0, copyPack.Count)];
                var createdCard = AddCardToDeck(cardData, i);
                
                copyPack.Remove(cardData);
                CardPack.Add(createdCard);
            }
        }

        private Card AddCardToDeck(CardDataSO cardData, int slotId)
        {
            Card card = Instantiate(_cardPrefab, _cardPositions[slotId]);
            card.GetComponent<RectTransform>().SetParent(_cardPositions[slotId]);
            card.gameObject.SetActive(true);
            card.Data = cardData;
            
            return card;
        }

        private List<CardDataSO> GetRandomCardsFromLInventory(List<CardDataSO> list, int number)
        {
            var tmpList = new List<CardDataSO>(list);
            var newList = new List<CardDataSO>();

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