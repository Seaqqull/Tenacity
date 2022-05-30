using UnityEngine.EventSystems;
using UnityEngine;


namespace Tenacity.Cards.Inventory
{
    public class InventorySlotsController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Card _cardView;
        [SerializeField] private InventoryCardDeck _cardDeck;


        private void Start()
        {
            if (_cardView != null)
                _cardView.gameObject.SetActive(false);
        }

        
        public void OnPointerDown(PointerEventData eventData)
        {
            var go = eventData.pointerCurrentRaycast.module.gameObject;

            if (go != null)
            {
                if (go.GetComponent<Card>())
                {
                    _cardView.GetComponent<Card>().Data = go.GetComponent<Card>().Data;
                    _cardView.GetComponent<CardDataDisplay>().DisplayCardValues();
                    _cardView.gameObject.SetActive(true);

                    if (_cardDeck != null)
                        _cardDeck.AddCardIntoCardDeck(go.GetComponent<Card>().Data);
                }
                //... for different items
            }
            else
            {
                _cardView.gameObject.SetActive(false);
            }
        } 
    }
}