using System.Collections;
using Tenacity.Battle;
using Tenacity.Draggable;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tenacity.Cards
{
    public class CardDeckItemController : MonoBehaviour, IPointerDownHandler
    {
        private CardDeckPlacingController _cardDeck;


        private void Awake()
        {
            if (GetComponentInParent<CardDeckPlacingController>())
            {
                _cardDeck = transform.root.GetComponentInChildren<CardDeckPlacingController>();
            }
        }


        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (!_cardDeck.enabled) return;
            if (_cardDeck.IsCurrentlyPlacingCard) return;

            _cardDeck.SelectCard(gameObject.GetComponent<Card>());
        }
    }
}