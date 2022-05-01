﻿using Tenacity.Battles.Draggable;
using Tenacity.Battles.Data;
using Tenacity.Battles;
using UnityEngine;


namespace Tenacity.Cards
{
    public class CardDeckPlacingController : MonoBehaviour
    {
        [SerializeField] private BattleManager _battle;
        [SerializeField] private RayPointerController _rayPointerController;
        
        private BattlePlayerController _player;
        private Card _currentlySelectedCard;
        
        public bool IsCurrentlyPlacingCard => _player.CurrentPlayerMode == PlayerActionMode.PlacingCard;
        public Card CurrentlySelectedCard
        {
            get => _currentlySelectedCard;
            set
            {
                _currentlySelectedCard = value;
                _player.CurrentPlayerMode = (value != null) ? PlayerActionMode.PlacingCard : PlayerActionMode.None;
            }
        }


        private void Awake()
        {
            _player = _battle.Player;
        }

        private void Update()
        {
            if ((CurrentlySelectedCard != null) && 
                (_player.CurrentPlayerMode == PlayerActionMode.None))
            {
                CurrentlySelectedCard = null;
            }
        }

        
        public void SelectCard(Card card)
        {
            CurrentlySelectedCard = card;
            var cardRectTransform = card.GetComponent<RectTransform>();
            
            _rayPointerController.StartPosition = (cardRectTransform.TransformPoint(cardRectTransform.rect.center));
        }
    }
}