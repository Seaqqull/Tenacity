using System.Collections;
using System.Collections.Generic;
using Tenacity.Cards;
using Tenacity.Draggable;
using UnityEngine;

namespace Tenacity.Battle
{
    public class BattlePlayerController : MonoBehaviour
    {
        [SerializeField] private CardDeckManager playerCardDeck;
        [SerializeField] private DraggableBattleCardController playerDraggableCardsController;

        private List<Card> _playerCards;

        private void Start()
        {
            if (playerCardDeck == null || playerCardDeck.CardPack.Count == 0) return;
            if (playerDraggableCardsController == null) return;
            _playerCards = playerCardDeck.CardPack;
            _playerCards.ForEach(el => el.IsDraggable = true);
        }

        public void SelectCardMode(bool isEnable)
        {
            playerDraggableCardsController.gameObject.SetActive(isEnable);
        }

        //tmp
        public bool IsGameOver()
        {
            int count = _playerCards.FindAll((c) => c.State == CardState.InCardDeck).Count;
            Debug.Log(count);
            Debug.Log(_playerCards.FindAll((c) => c.State == CardState.InCardDeck));
            if (count == 0) return true;
            return false;
        }
    }
}