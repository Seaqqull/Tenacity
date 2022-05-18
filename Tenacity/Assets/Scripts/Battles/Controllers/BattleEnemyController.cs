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

using static Tenacity.Battles.Lands.BattleConstants;
using Tenacity.Battles.Enemies;

namespace Tenacity.Battles.Controllers
{
    public class BattleEnemyController : MonoBehaviour
    {
        [SerializeField] private BattleCharacterSO _enemy;
        [SerializeField] private Vector3 _enemyPos;
        [SerializeField] private BattleCardDeckManager _enemyCardDeck;
        [SerializeField] private LandTypesSO _landTypes;
        [SerializeField] private TextMeshProUGUI _manaUI;
        [SerializeField] private float _yPos = 0.61f;
        [SerializeField] private Material _enemyMaterial;
        [SerializeField] private bool _isEnemyPlayer;
        [SerializeField] private BattleEnemyAI _enemyAI;


        private Card _heroCard;
        private int _currentMana;
        private List<Card> _enemyCards => _enemyCardDeck.CardPack;
        private List<Card> _allBattleMinions = new();
        private Dictionary<LandType, int> _landCounts;
        private List<Land> _enemyLandDeck => _landTypes.LandReferences;
        private HeroLandCellsController _availableLandCellsController = new();


        public Card Hero => _heroCard;
        public Dictionary<LandType, int> LandCounts => _landCounts;
        public List<Card> AvailableHandCards => _enemyCards.FindAll(card =>
            card.State == CardState.InCardDeck
            && _currentMana >= card.Data.CastingCost);
        public List<Land> FreeAvailableLands => _availableLandCellsController.FreeAvailableLands;
        public List<Card> Minions => _enemyCards.FindAll(card => card.State == CardState.OnBoard);
        public List<Card> Hand => _enemyCards.FindAll(card => card.State == CardState.InCardDeck);

        public int MinionsRating
        {
            get
            {
                int sum = 0;
                foreach (Card minion in Minions) sum += CountMinionRating(minion);
                return sum;
            }
        }   
        public int AdditionalRating => 
            BattleEnemyTrainController.Instance.IsGameOver
            ? ((IsGameOver ? -1 : 1) * GameStateRatings.HeroIsDeadRating)
            : 0;
        public int HeroRating => _heroCard.Data.CardRating;
        public int PersonalRating => BoardRating + HandRating;
        public int BoardRating => MinionsRating + HeroRating + LandRating;
        public int LandRating => GetNonWastedLandsAround(_heroCard).Count();
        public int HandRating => Hand.Count * GameStateRatings.HandCardWeightPerHandCard;

        public bool IsGameOver => (_heroCard.CurrentLife <= 0 || _enemyCards.Count <= 0);
        public List<Land> AvailableLandCells => _availableLandCellsController.AvailableLands.ToList();


        private void Awake()
        {
            _enemyAI.Player = this;
            _landCounts = _enemyLandDeck.GroupBy(x => x.Type).ToDictionary(x => x.Key, x => 0);
        }

        public void Init(Land startLand)
        {
            var enemy = Instantiate(_enemy.CharacterPrefab, startLand.transform);
            _heroCard = enemy.GetComponent<Card>();
            _heroCard.Data = _enemy;
            _heroCard.CurrentLife = _enemy.Life;

            enemy.transform.localPosition = _enemyPos;
            _availableLandCellsController.AddAvailableLand(startLand);
        }
        private void OnTurnStart()
        {
            _enemyCardDeck.ClearEmpty();
            _enemyCards.ForEach(card => card.IsAvailable = true);
            _enemyCardDeck.AddNewRandomCards();

            UpdateMana(BattleConstants.ROUND_MANA, false);
        }
        private void ManageCards()
        {
            _enemyAI.ManageAllCardsActions();
        }

        private void UpdateMana(int dtMana, bool isReduced)
        {
            _currentMana += (isReduced ? -dtMana : dtMana);
            _manaUI.text = "Mana: " + _currentMana;
        }
 

        public void PlaceLands(LandType landType, Land targetLand)
        {
            for (int i = 0; i < LandConstants.GetLandCellsCount(landType); i++)
                PlaceLandOnBoard(landType, targetLand);
        }
        public bool Attack(Card minion, Card attackedEnemy)
        {
            if (minion == null || attackedEnemy == null) return false;
            
            int figthBack = attackedEnemy.Data.Power;
            attackedEnemy.GetDamaged(minion.Data.Power);
            minion.GetDamaged(figthBack);
            return true;
        }
        public void MoveMinion(Card selectedMinion, Land selectedLand)
        {
            if (selectedLand == null) return;
            if ((selectedMinion.Transform.parent != null ? selectedMinion.GetComponentInParent<Land>() : null) != null)
                selectedMinion.Transform.SetParent(selectedLand.gameObject.transform);
            selectedMinion.Transform.localPosition = new Vector3(0, _yPos, 0);
        }
        public bool PlaceLandOnBoard(LandType landType, Land targetLand)
        {
            if (targetLand == null) return false;
            
            Land landToPlace = _enemyLandDeck.First(land => land.Type == landType);
            targetLand.ReplaceLand(landToPlace, !_isEnemyPlayer);
            _availableLandCellsController.AddAvailableLand(targetLand);
            _landCounts[landToPlace.Type]++;
            return true;
        }
        public void PlaceHandCardOnBoard(Card selectedCard, Land selectedLand)
        {
            if (selectedCard.Data.CastingCost > _currentMana) return;
            if (selectedCard.Data.LandCost > _landCounts[selectedCard.Data.Land]) return;
            if (selectedLand == null) return;

            Card creatureCard = CardManager.CreateCardCreatureOnBoard(selectedCard, selectedLand);
            creatureCard.GetComponent<MeshRenderer>().materials = new Material[] { _enemyMaterial };
            creatureCard.IsAvailable = false;

            _enemyCards.Remove(selectedCard);
            _enemyCards.Add(creatureCard);
            _allBattleMinions.Add(creatureCard);

            UpdateMana(selectedCard.Data.CastingCost, true);
        }

        public List<Land> GetNonWastedLandsAround(Card card)
        {
            Land land = card.GetComponentInParent<Land>();
            return land.NeighborLands.FindAll(l => l.Type != LandType.None).ToList();
        }
        public List<Card> GetCreaturesToAttack(Land cardPlace)
        {
            if (cardPlace == null) return null;
            return cardPlace.NeighborLands
                    .Select(el => el.GetComponentInChildren<Card>())
                    .ToList()
                    .FindAll(el =>  el != null
                        && !_enemyCards.Contains(el)
                        && el != Hero )
                    .ToList();
        }
        public List<Card> GetNearestMinions(Land cardPlace)
        {
            if (cardPlace == null) return null;
            return cardPlace.NeighborLands
                    .Select(el => el.GetComponentInChildren<Card>())
                    .ToList()
                    .FindAll(el => el != null
                        && _enemyCards.Contains(el))
                    .ToList();
        }

        public int CountMinionRating(Card minion)
        {
            if (minion == null) return 0;
            return minion.Data.CardRating + CountEnemiesInRangeRating(minion.GetComponentInParent<Land>());
        }
        public int CountMinionsInRangeRating(Land land)
        {
            if (land == null) return 0;
            var res = GetNearestMinions(land).Select(el =>
               GameStateRatings.MinionMovedRating).Sum();
            return (int)(res/2);
        }
        public int CountEnemiesInRangeRating(Land land)
        {
            if (land == null) return 0;
            return GetCreaturesToAttack(land).Select(el =>
                    (el.Data.Type == CardType.Hero)
                    ? GameStateRatings.MinionHasEnemyHeroInRangeRating
                    : GameStateRatings.MinionHasEnemyInRange).Sum();
        }

        public IEnumerator MakeMove(float time)
        {
            if (IsGameOver) yield return null;

            OnTurnStart();
            yield return new WaitForSeconds(time);

            ManageCards();
            yield return new WaitForSeconds(time);
        }
    }
}