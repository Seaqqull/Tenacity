using Tenacity.Battles.Lands.Data;
using System.Collections.Generic;
using Tenacity.Cards.Managers;
using Tenacity.Battles.Lands;
using Tenacity.Cards.Data;
using System.Collections;
using Tenacity.Cards;
using System.Linq;
using UnityEngine;
using TMPro;


namespace Tenacity.Battles.Controllers
{
    public class BattleEnemyController : MonoBehaviour
    {
        [SerializeField] private BattleCharacterSO _enemy;
        [SerializeField] private Vector3 _enemyPos;
        [SerializeField] private CardDeckManager _enemyCardDeck;
        [SerializeField] private List<Land> _enemyLandDeck;
        [SerializeField] private TextMeshProUGUI _manaUI;
        [SerializeField] private float _yPos = 0.61f;
        [SerializeField] private float _skipPobability;
        [SerializeField] private Material _enemyMaterial;


        private List<Card> _enemyCards = new ();
        private PlayerLandCellsController _availableLandCellsController = new();
        private List<Land> AvailableLandCells => _availableLandCellsController.AvailableLands.ToList();


        private int _currentMana;

        private Card _enemyCard;
        private Dictionary<LandType, int> _landCounts;


        public bool IsGameOver => _enemyCard.CurrentLife <= 0;


        private void Awake()
        {
            _enemyCards = _enemyCardDeck.CardPack;
            _landCounts = _enemyLandDeck.GroupBy(x => x.Type).ToDictionary(x => x.Key, x => 0);
        }

        public void Init(Land startLand)
        {
            var enemy = Instantiate(_enemy.CharacterPrefab, startLand.transform);
            _enemyCard = enemy.GetComponent<Card>();
            _enemyCard.Data = _enemy;
            _enemyCard.CurrentLife = _enemy.Life;

            enemy.transform.localPosition = _enemyPos;
            _availableLandCellsController.AddAvailableLand(startLand);
        }


        private bool DecideToMove()
        {
            return Random.value > _skipPobability; // alg.
        }
        private bool DecideToAttack()
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
        private bool PlaceLandOnBoard()
        {
            Land landToPlace = SelectLandFromDeck();
            Land targetPlace = SelectCellToPlaceLand(AvailableLandCells);
            if (targetPlace == null || landToPlace == null) return false;

            targetPlace.ReplaceLand(landToPlace, true);
            _availableLandCellsController.AddAvailableLand(targetPlace);
            _landCounts[landToPlace.Type]++;
            return true;
        }

        private Land SelectLandFromDeck()
        {
            if (_enemyLandDeck.Count == 0) return null;

            var cards = _enemyCards.FindAll(el => !el.IsPlaced);
            if (cards.Count == 0) cards = _enemyCards;

            List<LandType> availableCardTypes = _enemyCards
                .GroupBy(card => card.Data.Land)
                .Select(o => o.FirstOrDefault().Data.Land)
                .ToList();
            List<Land> filteredLands = _enemyLandDeck
                .FindAll(land => availableCardTypes.Contains(land.Type));

            if (filteredLands.Count == 0) return null;
            var randomNum = Random.Range(0, filteredLands.Count);
            Land land = filteredLands[randomNum];
            
            return land;
        }
        
        private Land SelectCellToPlaceLand(List<Land> landList)
        {
            List<Land> emptyLands = AvailableLandCells.FindAll((el) => el.Type == LandType.None && !el.IsStartPosition);
            if (emptyLands.Count == 0) return null;

            int landId = Random.Range(0, emptyLands.Count);
            Land landToPlace = emptyLands[landId];

            return landToPlace;
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
                    .FindAll(el => el.GetComponentInChildren<Card>() != null 
                    && !_enemyCards.Contains(el.GetComponentInChildren<Card>())
                    && el.GetComponentInChildren<Card>() != _enemyCard)
                    .Select(el => el.GetComponentInChildren<Card>())
                    .ToList();
        }


        //move a card
        private void MoveCard(Card selectedCard)
        {
            Land selectedLand = SelectLandToPlaceCard(selectedCard.transform.parent.GetComponent<Land>().NeighborLands, selectedCard.Data.Land);
            if (selectedLand == null) return;
            
            DropCardIntoLand(selectedCard, selectedLand);
        }


        //place a card (replace)
        private void PlaceCard(Card selectedCard)
        {
            if (selectedCard.Data.CastingCost > _currentMana) return;
            if (selectedCard.Data.LandCost > _landCounts[selectedCard.Data.Land]) return;

            Land selectedLand = SelectLandToPlaceCard(AvailableLandCells, selectedCard.Data.Land);
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
            if ((selectedCard.Transform.parent != null ? selectedCard.Transform.parent.GetComponent<Land>() : null) != null)
                selectedCard.Transform.SetParent(selectedLand.gameObject.transform);
            selectedCard.Transform.localPosition = new Vector3(0, _yPos, 0);
        }

        private Land SelectLandToPlaceCard(List<Land> landList, LandType cardLandType)
        {
            if (landList == null) return null;

            List<Land> availableLands = landList
                .FindAll((el) => el.IsAvailableForCards
                    && el.Type.HasFlag(cardLandType));
            if (availableLands.Count == 0) return null;

            Land selectedLand = availableLands.FindAll(cell => cell.NeighborLands.Any(el => !availableLands.Contains(el))).FirstOrDefault();
            if (selectedLand == null)
            {
                int landId = Random.Range(0, availableLands.Count);
                selectedLand = availableLands[landId];
            }
            return selectedLand;
        }


        private void TryMakeMove(Card selectedCard)
        {
            if ((selectedCard == null) || !selectedCard.IsAvailable) return;

            if (selectedCard.State == CardState.OnBoard)
            {
                bool isDamaged = false;
                if (DecideToAttack()) isDamaged = Attack(selectedCard);
                if (!isDamaged) MoveCard(selectedCard);
            }
            else if (selectedCard.State == CardState.InCardDeck)  PlaceCard(selectedCard);
        }

        private void PlaceLands()
        {
            Land selectedLand = SelectLandFromDeck();
            for (int i = 0; i < BattleConstants.LandConstants.GetLandCellsCount(selectedLand.Type); i++)
                PlaceLandOnBoard();
        }
        private void OnTurnStart()
        {
            _enemyCards = _enemyCards.Where(item => item != null).ToList();
            _enemyCards.ForEach(card => card.IsAvailable = true);
            
            UpdateMana(BattleConstants.ROUND_MANA, false);
        }
        private void ManageCards()
        {
            for (int i = 0; i < _enemyCards.Count; i++)
            {
                TryMakeMove(_enemyCards[i]);
                if (_currentMana == 0) break;
            }
        }


        public IEnumerator MakeMove(float time)
        {
            if (IsGameOver) yield return null;

            OnTurnStart();
            PlaceLands();
            yield return new WaitForSeconds(time);
            
            ManageCards();
            yield return new WaitForSeconds(time);
        }
    }
}