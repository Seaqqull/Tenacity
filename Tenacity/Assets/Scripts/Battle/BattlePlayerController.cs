using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tenacity.Cards;
using Tenacity.Draggable;
using Tenacity.Lands;
using TMPro;
using UnityEngine;

namespace Tenacity.Battle
{
    public class BattlePlayerController : MonoBehaviour
    {
        public enum PlayerActionMode
        {
            None,
            PlacingLand,
            PlacingCard,
            MovingCreature
        }

        [SerializeField] private CardDeckManager _playerCardDeck;
        [SerializeField] private DraggableCardController _playerDraggableCardsController;
        [SerializeField] private LandDeck _playerLandDeck;
        [SerializeField] private TextMeshProUGUI _manaUI;

        private Land _currentlySelectedLand;        //move into LandDeckController
        private int _availableCardLandsCount;       //move into LandDeckController
        private List<Card> _playerCards => _playerCardDeck?.CardPack;

        public int CurrentMana
        {
            get;
            private set;
        }
        public Land CurrentlySelectedLand
        {
            get => _currentlySelectedLand;
            set
            {
                CurrentPlayerMode = BattlePlayerController.PlayerActionMode.PlacingLand;
                _currentlySelectedLand = value;
                _availableCardLandsCount = BattleRules.LandRules.GetLandCellsCount(value.Type);
            }
        }
        public PlayerActionMode CurrentPlayerMode
        {
            get; set;
        }
        public bool IsGameOver => _playerCards.Count == 0;


        private void Start()
        {
            if (_playerDraggableCardsController == null) return;
            if (_playerCardDeck == null || _playerCardDeck.CardPack.Count == 0) return;
            _playerCards.ForEach(el => el.IsDraggable = true);
        }

        //TMP (methods dupl.)
        private void UpdateMana(int dtMana, bool isReduced)
        {
            CurrentMana += (isReduced ? -dtMana : dtMana);
            _manaUI.text = "Mana: " + CurrentMana;
        }

        public List<Card> GetCreaturesToAttack(Card selectedCard)
        {
            if (selectedCard == null) return null;
            Land parentLand = selectedCard.transform.parent?.GetComponent<Land>();
            return parentLand.NeighborLands
                    .FindAll(el => el.GetComponentInChildren<Card>() != null && !_playerCards.Contains(el.GetComponentInChildren<Card>()))
                    .Select(el => el.GetComponentInChildren<Card>())
                    .ToList();
        }

        //move into card controller
        public void Attack(Card selectedCard, Card cardToAttack)
        {
            if (cardToAttack == null) return;

            int figthBack = cardToAttack.Data.Power;
            cardToAttack.GetDamaged(selectedCard.Data.Power);
            selectedCard.GetDamaged(figthBack);
            selectedCard.IsDraggable = false;
        }

        public void OnClickDisable()
        {
            if (_availableCardLandsCount != BattleRules.LandRules.GetLandCellsCount(_currentlySelectedLand.Type)) return;
            _currentlySelectedLand = null;
            CurrentPlayerMode = PlayerActionMode.None;
        }
        public void AddNewCard(Card card)
        {
            _playerCards.Add(card);
            if (card.State == CardState.OnBoard) UpdateMana(card.Data.CastingCost, true);
        }
        public void RemoveCard(Card card)
        {
            _playerCards.Remove(card);
        }
        public void SelectCardMode(bool isEnable)
        {
            _playerDraggableCardsController.gameObject.SetActive(isEnable);
            if (_playerCards != null)
            {
                for (int i = 0; i < _playerCards.Count; i++)
                {
                    if (_playerCards[i] == null) _playerCards.RemoveAt(i);
                    else _playerCards[i].IsDraggable = isEnable;
                }
            }
            if (isEnable) UpdateMana(BattleRules.ROUND_MANA, false);
            
            _playerLandDeck.enabled = isEnable;
        }
        public void DecreaseAvailableCardsCount()
        {
            _availableCardLandsCount--;
            if (_availableCardLandsCount <= 0)
            {
                _currentlySelectedLand = null;
                _playerLandDeck.enabled = false;
                CurrentPlayerMode = PlayerActionMode.None;
            }
        }

    }
}