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
        [SerializeField] private Material _enemyMaterial;

        private int _currentMana;
        private List<Card> _enemyCards;

        public bool IsGameOver => _enemyCards.Where(item => item != null).Count() == 0;


        private void Start()
        {
            _enemyCards = _enemyCardDeck.CardPack;
        }

        private bool DecideToAttak()
        {
            return Random.value > _skipPobability; // alg.
        }
        private bool DecideToMove()
        {
            return Random.value > _skipPobability; // alg.
        }

        //common
        private void UpdateMana(int dtMana, bool isReduced)
        {
            _currentMana += (isReduced ? -dtMana : dtMana);
            _manaUI.text = "Mana: " + _currentMana;
        }


        //place a land
        private Land SelectLandFromDeck()
        {
            if (_enemyLandDeck.Count == 0) return null;
            List<LandType> availableCardTypes = _enemyCards.GroupBy(card => card.Data.Land).Select(o => o.FirstOrDefault().Data.Land).ToList();
            List<Land> filteredLands = _enemyLandDeck.FindAll(land => availableCardTypes.Contains(land.Type));
            if (filteredLands.Count == 0) return null;
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

            targetPlace.ReplaceLand(landToPlace);
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
            return true;
        }

        private List<Card> GetCreaturesToAttack(Card selectedCard)
        {
            if (selectedCard == null) return null;
            Land land = selectedCard.transform.parent?.GetComponent<Land>();
            return land.NeighborLands?
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
            if (selectedCard.Data.CastingCost > _currentMana) return;

            Land selectedLand = SelectLandToPlaceCard(places, selectedCard.Data.Land);
            if (selectedLand == null) return;

            Card creatureCard = CardManager.CreateCardCreatureOnBoard(selectedCard, selectedLand);
            creatureCard.GetComponent<MeshRenderer>().materials = new Material[] { _enemyMaterial };
            creatureCard.IsAvailable = false;

            _enemyCards.Remove(selectedCard);
            _enemyCards.Add(creatureCard);
            UpdateMana(selectedCard.Data.CastingCost, true);
        }

        private void DropCardIntoLand(Card selectedCard, Land selectedLand)
        {
            if (selectedCard.gameObject.transform.parent?.GetComponent<Land>() != null)
            selectedCard.gameObject.transform.SetParent(selectedLand.gameObject.transform);
            selectedCard.gameObject.transform.localPosition = new Vector3(0, _yPos, 0);
        }

        private Land SelectLandToPlaceCard(List<Land> landList, LandType cardLandType)
        {
            if (landList == null) return null;
            List<Land> availableLands = landList.FindAll((el) => el.IsAvailableForCards && el.Type.HasFlag(cardLandType));
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
            if (selectedCard == null || !selectedCard.IsAvailable) return;

            if (selectedCard.State == CardState.OnBoard)
            {
                bool isDamaged = false;
                if (DecideToAttak()) 
                    isDamaged = Attack(selectedCard);
                if (!isDamaged) 
                    MoveCard(selectedCard);
            }
            else if (selectedCard.State == CardState.InCardDeck)
            {
                PlaceCard(selectedCard, places);
            }
        }

        private void OnStartTurn()
        {
            _enemyCards = _enemyCards.Where(item => item != null).ToList();
            _enemyCards.ForEach(card => card.IsAvailable = true);
            UpdateMana(BattleConstants.ROUND_MANA, false);
        }

        private void PlaceLands(List<Land> places)
        {
            Land selectedLand = SelectLandFromDeck();
            for (int i = 0; i < BattleConstants.LandConstants.GetLandCellsCount(selectedLand.Type); i++)
            {
                PlaceLandOnBoard(places);
            }
        }

        private void ManageCards(List<Land> places)
        {
            for (int i = 0; i < _enemyCards.Count; i++)
            {
                TryMakeMove(_enemyCards[i], places);
                if (_currentMana == 0) break;
            }
        }

        public IEnumerator MakeMove(List<Land> places, float time)
        {
            if (IsGameOver) yield return null;

            OnStartTurn();
            PlaceLands(places);
            yield return new WaitForSeconds(time);
            
            ManageCards(places);
            yield return new WaitForSeconds(time);
        }
    }
}