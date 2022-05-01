using UnityEngine.EventSystems;
using UnityEngine;


namespace Tenacity.Cards
{
    public class CardDeckItemController : MonoBehaviour, IPointerDownHandler
    {
        private CardDeckPlacingController _cardDeck;


        private void Awake()
        {
            if (GetComponentInParent<CardDeckPlacingController>() != null)
                _cardDeck = transform.root.GetComponentInChildren<CardDeckPlacingController>();
        }


        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if ((_cardDeck == null) || (!_cardDeck.enabled) || (_cardDeck.IsCurrentlyPlacingCard)) return;

            _cardDeck.SelectCard(gameObject.GetComponent<Card>());
        }
    }
}