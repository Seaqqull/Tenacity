using System.Collections;
using System.Collections.Generic;
using Tenacity.Cards;
using Tenacity.Lands;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Battle
{
    public class BattleEnemyAIController : MonoBehaviour
    {
        [SerializeField] private CardDeckManager enemyCardDeck;
        [SerializeField] private List<Land> enemyLandDeck = new List<Land>();
        [SerializeField] private float yPos = 0.61f;

        //tmp 'public'
        public List<Card> _enemyCards;

        private void Start()
        {
            if (enemyCardDeck == null || enemyCardDeck.CardPack.Count == 0) return;
            _enemyCards = enemyCardDeck.CardPack;
        }

        private Card SelectCard()
        {
            if (_enemyCards.Count == 0) return null;
            int cardId = Random.Range(0, _enemyCards.Count);
            Card selectedCard = _enemyCards[cardId];
            return selectedCard;
        }

        private Card SelectCardByLandType(LandType type)
        {
            if (_enemyCards.Count == 0) return null;
            List<Card> availableCards = _enemyCards.FindAll((el) => el.Data.Land == type);
            if (availableCards.Count == 0) return null;
            int cardId = Random.Range(0, availableCards.Count);
            Card selectedCard = availableCards[cardId];
            return selectedCard;
        }

        private Land TakeLandFromDeck()
        {
            if (enemyLandDeck.Count == 0) return null;
            Land land = enemyLandDeck[^1];

            return land;
        }

        private Land SelectAvailableLandByType(List<Land> landList, LandType landType)
        {
            List<Land> availableLands = new List<Land>();
            if (landType != LandType.Neutral)
                availableLands = landList.FindAll((el) => el.IsAvailableForCards && el.Type == landType);
            else
                availableLands = landList.FindAll((el) => el.IsAvailableForCards);

            int landId = Random.Range(0, availableLands.Count);
            if (availableLands.Count == 0) return null;

            Land landToPlace = availableLands[landId];
            return landToPlace;
        }

        private Land SelectEmptyLand(List<Land> landList)
        {
            List<Land> emptyLands = landList.FindAll((el) => el.Type == LandType.None);
            int landId = Random.Range(0, emptyLands.Count);
            if (emptyLands.Count == 0) return null;

            Land landToPlace = emptyLands[landId];
            return landToPlace;
        }

        public bool PlaceLandOnBoard(List<Land> availablePlaces, float time)
        {
            Land landToPlace = TakeLandFromDeck();
            Land targetPlace = SelectEmptyLand(availablePlaces);
            if (targetPlace == null || landToPlace == null) return false;

            enemyLandDeck.Remove(landToPlace);
            landToPlace.transform.SetParent(targetPlace.transform);

            targetPlace.ReplaceEmptyLand(landToPlace);
            return true;
        }

        public bool PlaceCardIntoLand(List<Land> availablePlaces, float time)
        {
            if (availablePlaces == null) return false;
            
            Card selectedCard = SelectCard();
            if (selectedCard == null) return false;

            Land selectedLand = SelectAvailableLandByType(availablePlaces, selectedCard.Data.Land);
            if (selectedLand == null)
            {
                selectedLand = SelectAvailableLandByType(availablePlaces, LandType.Neutral);
                if (selectedLand == null) return false;

                selectedCard = SelectCardByLandType(selectedLand.Type);
                if (selectedCard == null) return false;
            }

            _enemyCards.Remove(selectedCard);
            selectedCard.gameObject.transform.SetParent(selectedLand.gameObject.transform);
            selectedCard.gameObject.transform.localPosition = new Vector3(0, yPos, 0);
            selectedLand.IsAvailableForCards = false;

            BattleCardController.CreateCardCreatureOnBoard(selectedCard);
            _enemyCards.Remove(selectedCard);
            return true;
        }

        //tmp
        public bool IsGameOver()
        {
            if (_enemyCards.Count == 0) return true;
            return false;
        }
    }
}