using UnityEngine.EventSystems;
using UnityEngine;


namespace Tenacity.Cards.Inventory
{
    public class InventorySlotsController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private CardItem _cardView;
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
                if (go.GetComponent<CardItem>())
                {
                    _cardView.GetComponent<CardItem>().Data = go.GetComponent<CardItem>().Data;
                    _cardView.GetComponent<CardDataDisplay>().DisplayCardValues();
                    _cardView.gameObject.SetActive(true);

                    if (_cardDeck != null)
                        _cardDeck.AddCardIntoCardDeck(go.GetComponent<CardItem>().Data);
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