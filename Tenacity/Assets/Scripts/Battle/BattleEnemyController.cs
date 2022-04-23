using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tenacity.Cards;
using Tenacity.Lands;
using TMPro;
using UnityEngine;


namespace Tenacity.Battle
{
    public class BattleEnemyController : MonoBehaviour
    {
        [SerializeField] private BattleEnemy _enemy;
        [SerializeField] private CardDeckManager _enemyCardDeck;
        [SerializeField] private List<Land> _enemyLandDeck = new List<Land>();
        [SerializeField] private TextMeshProUGUI _manaUI;
        [SerializeField] private float _yPos = 0.61f;
        [SerializeField] private float _skipPobability;

        private List<Card> _enemyCards;

        public bool IsGameOver => _enemyCards.Count == 0;


        private void Start()
        {
            if (_enemyCardDeck == null || _enemyCardDeck.CardPack.Count == 0) return;
            _enemyCards = _enemyCardDeck.CardPack;
        }

        private bool DecideToAttak()
        {
            return Random.value > _skipPobability;
        }
        private bool DecideToMove()
        {
            return Random.value > _skipPobability;
        }

        //common
        private void UpdateMana(int dtMana, bool isReduced)
        {
            _enemy.CurrentMana += (isReduced ? -dtMana : dtMana);
            _manaUI.text = "Mana: " + _enemy.CurrentMana;
        }


        //place a land
        private Land SelectLandFromDeck()
        {
            if (_enemyLandDeck.Count == 0) return null;
            List<LandType> availableCardTypes = _enemyCards.GroupBy(card => card.Data.Land).Select(o => o.FirstOrDefault().Data.Land).ToList();
            List<Land> filteredLands = _enemyLandDeck.FindAll(land => availableCardTypes.Contains(land.Type));
            var randomNum = Random.Range(0, filteredLands.Count);
            Land land = filteredLands[randomNum];
            return land;
        }
        private Land SelectCellToPlaceLand(List<Land> landList)
        {
            List<Land> emptyLands = landList.FindAll((el) => el.Type == LandType.None);
            if (emptyLands.Count == 0) return null;

            int landId = Random.Range(0, emptyLands.Count);
            Land landToPlace = emptyLands[landId];
            return landToPlace;
        }
        private bool PlaceLandOnBoard(List<Land> availablePlaces)
        {
            Land landToPlace = SelectLandFromDeck();
            Land targetPlace = SelectCellToPlaceLand(availablePlaces);
            if (targetPlace == null || landToPlace == null) return false;

            targetPlace.ReplaceEmptyLand(landToPlace);
            return true;
        }

        //(common) attack cards
        private bool Attack(Card selectedCard)
        {
            List<Card> creaturesToAttack = GetCreaturesToAttack(selectedCard);
            if (creaturesToAttack?.Count == 0) return false;

            Card cardToAttack = creaturesToAttack.OrderBy(el => el.Data.Power / el.Data.Life).FirstOrDefault();
            if (cardToAttack == null) return false;

            int figthBack = cardToAttack.Data.Power;
            cardToAttack.GetDamaged(selectedCard.Data.Power);
            selectedCard.GetDamaged(figthBack);
            selectedCard.IsDraggable = false;
            return true;
        }
        private List<Card> GetCreaturesToAttack(Card selectedCard)
        {
            if (selectedCard == null) return null;
            Land parentLand = selectedCard.transform.parent?.GetComponent<Land>();
            return parentLand.NeighborLands
                    .FindAll(el => el.GetComponentInChildren<Card>() != null && !_enemyCards.Contains(el.GetComponentInChildren<Card>()))
                    .Select(el => el.GetComponentInChildren<Card>())
                    .ToList();
        }

        //move a card
        private void MoveCard(Card selectedCard)
        {
            Land selectedLand = SelectNeighborLand(selectedCard.transform.parent?.GetComponent<Land>());
            if (selectedLand == null) return;
            DropCardIntoLand(selectedCard, selectedLand);
        }
        private Land SelectNeighborLand(Land land)
        {
            return SelectLandToPlaceCard(land.NeighborLands, land.Type);
        }


        //place a card (replace)
        private void PlaceCard(Card selectedCard, List<Land> places)
        {
            if (selectedCard.Data.CastingCost > _enemy.CurrentMana) return;

            Land selectedLand = SelectLandToPlaceCard(places, selectedCard.Data.Land);
            if (selectedLand == null) return;

            DropCardIntoLand(selectedCard, selectedLand);
            Card creatureCard = CardController.CreateCardCreatureOnBoard(selectedCard);
            _enemyCards.Remove(selectedCard);
            _enemyCards.Add(creatureCard);
            UpdateMana(selectedCard.Data.CastingCost, true);
        }
        private void DropCardIntoLand(Card selectedCard, Land selectedLand)
        {
            if (selectedCard.gameObject.transform.parent?.GetComponent<Land>() != null)
                selectedCard.gameObject.transform.parent.GetComponent<Land>().IsAvailableForCards = true;
            selectedCard.gameObject.transform.SetParent(selectedLand.gameObject.transform);
            selectedCard.gameObject.transform.localPosition = new Vector3(0, _yPos, 0);
            selectedLand.IsAvailableForCards = false;
            selectedCard.IsDraggable = false;
        }
        private Land SelectLandToPlaceCard(List<Land> landList, LandType cardLandType)
        {
            if (landList == null) return null;
            List<Land> availableLands = new List<Land>();
            availableLands = landList.FindAll((el) => el.IsAvailableForCards && el.Type.HasFlag(cardLandType));
            if (availableLands.Count == 0) return null;

            Land selectedLand = availableLands.FindAll(cell => cell.NeighborLands.Any(el => !availableLands.Contains(el))).FirstOrDefault();
            if (selectedLand == null)
            {
                int landId = Random.Range(0, availableLands.Count);
                selectedLand = availableLands[landId];
            }
            return selectedLand;
        }


        private void TryMakeMove(Card selectedCard, List<Land> places)
        {
            if (selectedCard == null) return;

            if (selectedCard.State == CardState.OnBoard)
            {
                bool isTakeDamage = false;
                if (DecideToAttak()) isTakeDamage = Attack(selectedCard);
                if (!isTakeDamage) MoveCard(selectedCard);
            }
            else if (selectedCard.State == CardState.InCardDeck)
            {
                PlaceCard(selectedCard, places);
            }
        }


        public IEnumerator MakeMove(List<Land> places, float time)
        {
            UpdateMana(BattleRules.ROUND_MANA, false);
            Land selectedLand = SelectLandFromDeck();
            for (int i = 0; i < BattleRules.LandRules.GetLandCellsCount(selectedLand.Type); i++)
            {
                PlaceLandOnBoard(places);
            }
            yield return new WaitForSeconds(time);
            for (int i = 0; i < _enemyCards.Count; i++)
            {
                TryMakeMove(_enemyCards[i], places);
                if (_enemy.CurrentMana == 0) break;
                yield return new WaitForSeconds(time);
            }
            yield return new WaitForSeconds(time);
        }
    }
}