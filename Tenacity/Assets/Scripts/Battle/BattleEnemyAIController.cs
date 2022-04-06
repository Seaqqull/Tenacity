using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tenacity.Cards;
using Tenacity.Lands;
using TMPro;
using UnityEngine;

/*
 * If there are player's cards near selected card, decide to move or to take damage.
 * */

namespace Tenacity.Battle
{
    public class BattleEnemyAIController : MonoBehaviour
    {
        [SerializeField] private BattleEnemy enemy;
        [SerializeField] private CardDeckManager enemyCardDeck;
        [SerializeField] private List<Land> enemyLandDeck = new List<Land>();
        [SerializeField] private TextMeshProUGUI manaUI;
        [SerializeField] private float yPos = 0.61f;
        [SerializeField] private float skipPobability;

        private List<Card> _enemyCards;

        private void Start()
        {
            if (enemyCardDeck == null || enemyCardDeck.CardPack.Count == 0) return;
            _enemyCards = enemyCardDeck.CardPack;
        }

        private bool DecideToAttak()
        {
            return true;
                //Random.value < skipPobability;
        }

        private Land TakeLandFromDeck()
        {
            if (enemyLandDeck.Count == 0) return null;
            Land land = enemyLandDeck[^1];

            return land;
        }

        private Land SelectNeighborLand(Land land)
        {
            List<Land> neighbors = land.NeighborLands;
            return SelectLandToPlaceCard(neighbors, land.Type);
        }

        private void UpdateMana(int dtMana, bool isReduced)
        {
            enemy.CurrentMana += (isReduced ? -dtMana : dtMana);
            manaUI.text = "Mana: " + enemy.CurrentMana;
        }
        
        private Land SelectCellToPlaceLand(List<Land> landList)
        {
            List<Land> emptyLands = landList.FindAll((el) => el.Type == LandType.None);
            int landId = Random.Range(0, emptyLands.Count);
            if (emptyLands.Count == 0) return null;

            Land landToPlace = emptyLands[landId];
            return landToPlace;
        }

        private bool PlaceLandOnBoard(List<Land> availablePlaces)
        {
            Land landToPlace = TakeLandFromDeck();
            Land targetPlace = SelectCellToPlaceLand(availablePlaces);
            if (targetPlace == null || landToPlace == null) return false;

            enemyLandDeck.Remove(landToPlace);
            landToPlace.transform.SetParent(targetPlace.transform);

            targetPlace.ReplaceEmptyLand(landToPlace);
            return true;
        }

        private void TakeDamage(Card selectedCard, List<Card> playerCreatures)
        {
            Card cardToAttack = playerCreatures
                        .OrderBy(el => el.Data.Power / el.Data.Life)
                        .FirstOrDefault();
            if (cardToAttack == null) return;

            int figthBack = cardToAttack.Data.Power;
            cardToAttack.GetDamaged(selectedCard.Data.Power);
            selectedCard.GetDamaged(figthBack);
        }

        private void TryMoveCard(Card selectedCard, List<Land> places)
        {
            if (selectedCard == null) return;

            if (selectedCard.State == CardState.OnBoard)
            {
                Land parentLand = selectedCard.transform.parent?.GetComponent<Land>();
                bool takeDamage = false;
                List<Card> neighborCreatures = parentLand.NeighborLands
                    .FindAll(el => el.GetComponentInChildren<Card>() != null && !_enemyCards.Contains(el.GetComponentInChildren<Card>())) 
                    .Select(el => el.GetComponentInChildren<Card>())
                    .ToList();
                if (neighborCreatures.Count != 0) takeDamage = DecideToAttak();

                if (takeDamage)
                {
                    TakeDamage(selectedCard, neighborCreatures);
                }
                else
                {
                    Land selectedLand = SelectNeighborLand(parentLand);
                    if (selectedLand == null) return;
                    DropCardIntoLand(selectedCard, selectedLand);
                }
            }
            else if (selectedCard.State == CardState.InCardDeck)
            {
                if (selectedCard.Data.CastingCost > enemy.CurrentMana) return;

                Land selectedLand = SelectLandToPlaceCard(places, selectedCard.Data.Land);
                if (selectedLand == null) return;

                DropCardIntoLand(selectedCard, selectedLand);
                Card creatureCard = CardController.CreateCardCreatureOnBoard(selectedCard);
                _enemyCards.Remove(selectedCard);
                _enemyCards.Add(creatureCard);
                UpdateMana(selectedCard.Data.CastingCost, true);
            }
        }

        private void DropCardIntoLand(Card selectedCard, Land selectedLand)
        {
            if (selectedCard.gameObject.transform.parent?.GetComponent<Land>() != null)
                selectedCard.gameObject.transform.parent.GetComponent<Land>().IsAvailableForCards = true;
            selectedCard.gameObject.transform.SetParent(selectedLand.gameObject.transform);
            selectedCard.gameObject.transform.localPosition = new Vector3(0, yPos, 0);
            selectedLand.IsAvailableForCards = false;
        }

        private Land SelectLandToPlaceCard(List<Land> landList, LandType cardLandType)
        {
            if (landList == null) return null;
            List<Land> availableLands = new List<Land>();
            availableLands = landList.FindAll((el) => el.IsAvailableForCards && el.Type.HasFlag(cardLandType));
            if (availableLands.Count == 0) return null;

            int landId = Random.Range(0, availableLands.Count);
            Land landToPlace = availableLands[landId];
            return landToPlace;
        }

        /*
        private Card SelectCardByLandType(LandType type, CardState cardState)
        {
            if (_enemyCards.Count == 0) return null;
            List<Card> availableCards = _enemyCards.FindAll( (el) => 
                (el.Data.Land.HasFlag(type)) &&  (el.State == cardState));
            if (availableCards.Count == 0) return null;
            int cardId = Random.Range(0, availableCards.Count);
            Card selectedCard = availableCards[cardId];
            return selectedCard;
        }
        */
        /*
        private bool MoveDeckCard(List<Land> availablePlaces)
        {
            Card selectedCard = SelectCardByLandType(LandType.None, CardState.InCardDeck);
            if (selectedCard == null) return false;
            Land selectedLand = SelectAvailableLandByType(availablePlaces, selectedCard.Data.Land);
            if (selectedLand == null) return false;
            
            DropCardIntoLand(selectedCard, selectedLand);
            Card creatureCard = CardController.CreateCardCreatureOnBoard(selectedCard);
            _enemyCards.Remove(selectedCard);
            _enemyCards.Add(creatureCard);
            return true;
        }

        private bool MoveCardOnBoard(List<Land> availablePlaces) 
        {
            Card selectedCard = SelectCardByLandType(LandType.None, CardState.OnBoard);
            if (selectedCard == null) return false;
            Land selectedLand = SelectNeighborLand(selectedCard.transform.parent?.GetComponent<Land>());
            if (selectedLand == null) return false;
            DropCardIntoLand(selectedCard, selectedLand);

            return true;
        }
        */
        /*
        public bool PlaceCardIntoLand(List<Land> availablePlaces)
        {
            if (availablePlaces == null) return false;

            bool isCardMoved = MoveDeckCard(availablePlaces);
            if (isCardMoved) return true;

            isCardMoved = MoveCardOnBoard(availablePlaces);
            
            return isCardMoved;
        }
        */

        public IEnumerator MakeMove(List<Land> places, float time)
        {
            UpdateMana(BattleRules.ROUND_MANA, false);
            for (int i = 0; i < BattleRules.LandRules.NEUTRAL; i++)
            {
                PlaceLandOnBoard(places);
            }
            yield return new WaitForSeconds(time);
            for (int i = 0; i < _enemyCards.Count; i++)
            {
                TryMoveCard(_enemyCards[i], places);
                if (enemy.CurrentMana == 0) break;
            }
            yield return new WaitForSeconds(time);
        }
        //tmp
        public bool IsGameOver()
        {
            if (_enemyCards.Count == 0) return true;
            return false;
        }
    }
}