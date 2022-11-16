using System.Collections.Generic;
using System.Linq;
using Tenacity.Cards.Inventory;
using UnityEngine;


namespace Tenacity.Cards
{
    public class BattleCardDeckManager : MonoBehaviour
    {
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private Transform[] _cardPositions;
        [SerializeField] private CardDecksSO _cardDecks;
        [SerializeField] private List<CardSO> _cardsDataPack = new List<CardSO>();

        private CardDeck _cardDeck => _cardDecks.CurrentlySelectedCardDeck;
        public List<Card> CardPack { get; private set; } = new List<Card>();


        private void Awake()
        {
            if ((_cardDecks.CurrentlySelectedCardDeck == null) || (_cardPositions == null)) return;

            _cardsDataPack = GetRandomCardsFromDeck(_cardDeck.Cards, _cardPositions.Length);
            InitCardDeck(_cardsDataPack);
        }

        
        private void InitCardDeck(List<CardSO> cardDataPack)
        {
            if (cardDataPack.Count <= 0) return;

            var copyPack = new List<CardSO>(cardDataPack);
            for (int i = 0; i < _cardPositions.Length; i++)
            {
                var cardData = copyPack[Random.Range(0, copyPack.Count)];
                var createdCard = CreateHandCardInDeck(cardData, i);
                
                copyPack.Remove(cardData);
                CardPack.Add(createdCard);
            }
        }

        private Card CreateHandCardInDeck(CardSO cardData, int slotId)
        {
            Card card = Instantiate(_cardPrefab, _cardPositions[slotId]);
            card.GetComponent<RectTransform>().SetParent(_cardPositions[slotId]);
            card.gameObject.SetActive(true);
            card.Data = cardData;
            card.State = Data.CardState.InCardDeck;
            
            return card;
        }

        private List<CardSO> GetRandomCardsFromDeck(List<CardSO> list, int number)
        {
            var tmpList = new List<CardSO>(list);
            var newList = new List<CardSO>();

            while (newList.Count < number)
            {
                int id = Random.Range(0, tmpList.Count);
                newList.Add(tmpList[id]);
            }
            return newList;
        }

        public void ClearEmpty()
        {
            CardPack = CardPack.Where(item => item != null).ToList();
        }
        public void AddNewRandomCards() {
            for (int i = 0; i < _cardPositions.Length; i++)
            {
                CardSO data = _cardDeck.Cards[Random.Range(0, _cardDeck.Cards.Count)];
                if (!_cardPositions[i].GetComponentInChildren<Card>())
                {
                    Card createdCard = CreateHandCardInDeck(data, i);
                    CardPack.Add(createdCard);
                    break;
                }
            }
        }

    }
}