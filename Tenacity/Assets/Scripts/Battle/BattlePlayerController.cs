using System.Collections.Generic;
using System.Linq;
using Tenacity.Cards;
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

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Vector3 _playerPos;

        [Header("Cards")]
        [SerializeField] private CardDeckManager _playerCardDeck;
        [SerializeField] private CardDeckPlacingController _cardDeckInputController;
        [SerializeField] private CreatureDragging _creatureDraggingController;

        [Header("Lands")]
        [SerializeField] private LandDeckPlacingController _landDeckInputController;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _manaUI;


        public int CurrentMana { get; private set; }
        public PlayerActionMode CurrentPlayerMode { get; set; }
        public bool IsGameOver => PlayerCards?.Count == 0;

        public List<Card> PlayerCards => _playerCardDeck?.CardPack;
        public LandDeckPlacingController LandDeckInputController { get => _landDeckInputController; }
        public CardDeckPlacingController CardDeckInputController { get => _cardDeckInputController; }


        private void Activate(bool mode)
        {
            _landDeckInputController.enabled = mode;
            _creatureDraggingController.enabled = mode;
            _cardDeckInputController.enabled = mode;
        }
        private void UpdateMana(int dtMana, bool isReduced)
        {
            CurrentMana += (isReduced ? -dtMana : dtMana);
            _manaUI.text = "Mana: " + CurrentMana;
        }


        public void Init(Land startLand)
        {
            var player = Instantiate(_playerPrefab, startLand.transform);
            player.transform.localPosition = _playerPos;
        }

        public bool PlaceDeckCardOnLand(Land land)
        {
            Card selectedCard = _cardDeckInputController.CurrentlySelectedCard;

            if (selectedCard.Data.CastingCost > CurrentMana) return false;
            if ( !(land.IsAvailableForCards && land.Type.HasFlag(selectedCard.Data.Land)) ) return false;

            PlayerCards.Remove(selectedCard);
            Card creature = CardManager.CreateCardCreatureOnBoard(selectedCard, land);
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

    }
}