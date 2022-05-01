using System.Collections;
using Tenacity.Battle;
using Tenacity.Draggable;
using UnityEngine;
using static Tenacity.Battle.BattlePlayerController;

namespace Tenacity.Cards
{
    public class CardDeckPlacingController : MonoBehaviour
    {
        [SerializeField] private BattleManager _battle;
        [SerializeField] private RayPointerController _rayPointerController;

        
        private Card _currentlySelectedCard;
        private BattlePlayerController _player;


        public Card CurrentlySelectedCard
        {
            get => _currentlySelectedCard;
            set
            {
                _currentlySelectedCard = value;
                _player.CurrentPlayerMode = (value != null) ? PlayerActionMode.PlacingCard : PlayerActionMode.None;
            }
        }
        public bool IsCurrentlyPlacingCard => _player.CurrentPlayerMode == PlayerActionMode.PlacingCard;


        private void Awake()
        {
            _player = _battle.Player;
        }

        private void Update()
        {
            if (CurrentlySelectedCard != null
                && _player.CurrentPlayerMode == PlayerActionMode.None)
            {
                CurrentlySelectedCard = null;
            }
        }

        public void SelectCard(Card card)
        {
            CurrentlySelectedCard = card;
            RectTransform cardRectTransform = card.GetComponent<RectTransform>();
            _rayPointerController.StartPosition = (cardRectTransform.TransformPoint(cardRectTransform.rect.center));
        }
    }
}