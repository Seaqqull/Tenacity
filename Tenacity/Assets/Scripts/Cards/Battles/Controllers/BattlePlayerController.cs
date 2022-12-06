using System.Collections.Generic;
using Tenacity.Cards.Managers;
using Tenacity.Battles.Lands;
using Tenacity.Battles.Data;
using Tenacity.Cards.Data;
using Tenacity.Cards;
using UnityEngine;
using TMPro;
using Tenacity.Battles.Lands.Data;
using System;
using System.Linq;

namespace Tenacity.Battles.Controllers
{
    public class BattlePlayerController : MonoBehaviour
    {
        [SerializeField] private BattleCharacterSO _player;
        [SerializeField] private Vector3 _playerPos;
        [Header("Cards")]
        [SerializeField] private BattleCardDeckManager _playerCardDeck;
        [SerializeField] private CardDeckPlacingController _cardDeckInputController;
        [SerializeField] private CreatureDragging _creatureDraggingController;
        [Header("Lands")]
        [SerializeField] private LandDeckPlacingController _landDeckInputController;
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _manaUI;


        private HeroLandCellsController _availableLandCellsController = new();
        private Dictionary<LandType, int> _landCounts;

        public HeroLandCellsController AvailableLandCellsController => _availableLandCellsController;
        public List<CardItem> PlayerCards => _playerCardDeck != null ? _playerCardDeck.CardPack : null;
        public LandDeckPlacingController LandDeckInputController => _landDeckInputController;
        public CardDeckPlacingController CardDeckInputController => _cardDeckInputController;
        public Dictionary<LandType, int> LandCounts => _landCounts;
        public PlayerActionMode CurrentPlayerMode { get; set; }
        public bool IsGameOver => (Player.CurrentLife <= 0 ||  PlayerCards.Count <= 0);
        public int CurrentMana { get; private set; }
        public CardItem Player { get; private set; }


        private void Awake()
        {
            _landCounts = Enum.GetValues(typeof(LandType)).Cast<LandType>().ToList().GroupBy(x => x).ToDictionary(x => x.Key, x => 0);
        }

        private void Activate(bool mode)
        {
            _creatureDraggingController.enabled = mode;
            _landDeckInputController.enabled = mode;
            _cardDeckInputController.enabled = mode;
        }
        
        private void UpdateMana(int dtMana, bool isReduced)
        {
            CurrentMana += (isReduced ? -dtMana : dtMana);
            _manaUI.text = "Mana: " + CurrentMana;
        }


        public void Init(Land startLand)
        {
            var player = Instantiate(_player.CharacterPrefab, startLand.transform);
            Player = player.GetComponent<CardItem>();
            Player.Data = _player;
            Player.CurrentLife = _player.Life;
            player.transform.localPosition = _playerPos;
            _availableLandCellsController.AddAvailableLand(startLand);
            
        }

        public bool PlaceDeckCardOnLand(Land land)
        {
            CardItem selectedCard = _cardDeckInputController.CurrentlySelectedCard;

            if (selectedCard.Data.CastingCost > CurrentMana) return false;
            if (!(land.IsAvailableForCards && land.Type.HasFlag(selectedCard.Data.Land)) ) return false;
            if (!_availableLandCellsController.IsLandAvailable(land)) return false;

            PlayerCards.Remove(selectedCard);
            CardItem creature = CardManager.CreateCardCreatureOnBoard(selectedCard, land);
            PlayerCards.Add(creature);

            if (creature.State == CardState.OnBoard) UpdateMana(creature.Data.CastingCost, true);
            return true;
        }

        public void SetActiveMode(bool isEnable)
        {
            if (PlayerCards != null)
                for (int i = 0; i < PlayerCards.Count; i++)
                    if (PlayerCards[i] == null) PlayerCards.RemoveAt(i);
                    else PlayerCards[i].enabled = isEnable;
            
            if (isEnable) UpdateMana(BattleConstants.ROUND_MANA, false);
            Activate(isEnable);
        }

        public void AddLandCount(LandType landType)
        {
            _landCounts[landType]++;
        }
    }
}