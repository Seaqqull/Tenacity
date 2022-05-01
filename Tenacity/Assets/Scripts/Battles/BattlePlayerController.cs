using System.Collections.Generic;
using Tenacity.Battles.Data;
using Tenacity.Battles.Lands;
using Tenacity.Cards.Data;
using Tenacity.Cards;
using UnityEngine;
using TMPro;


namespace Tenacity.Battles
{
    public class BattlePlayerController : MonoBehaviour
    {
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
        
        public LandDeckPlacingController LandDeckInputController { get => _landDeckInputController; }
        public CardDeckPlacingController CardDeckInputController { get => _cardDeckInputController; }
        public List<Card> PlayerCards => _playerCardDeck?.CardPack;
        public PlayerActionMode CurrentPlayerMode { get; set; }
        public bool IsGameOver => PlayerCards?.Count == 0;
        public int CurrentMana { get; private set; }
        

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
            var player = Instantiate(_playerPrefab, startLand.transform);
            player.transform.localPosition = _playerPos;
        }

        public bool PlaceDeckCardOnLand(Land land)
        {
            Card selectedCard = _cardDeckInputController.CurrentlySelectedCard;

            if (selectedCard.Data.CastingCost > CurrentMana) return false;
            if (!(land.IsAvailableForCards && land.Type.HasFlag(selectedCard.Data.Land)) ) return false;

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