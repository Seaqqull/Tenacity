using UnityEngine.EventSystems;
using UnityEngine;


namespace Tenacity.Cards.Inventory
{
    public class InventorySlotsController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject _detailsViewScreen;
        [SerializeField] private InventoryCardDeck _cardDeck;

        private Card _item;

        
        private void Start()
        {
            if (_detailsViewScreen == null) return;
            _item = _detailsViewScreen.GetComponentInChildren<Card>();
            _item.gameObject.SetActive(false);
        }

        
        public void OnPointerDown(PointerEventData eventData)
        {
            var go = eventData.pointerCurrentRaycast.module.gameObject;

            if (go != null)
            {
                if (go.GetComponent<Card>())
                {
                    _item.GetComponent<Card>().Data = go.GetComponent<Card>().Data;
                    _item.GetComponent<CardDataDisplay>().DisplayCardValues();
                    _item.gameObject.SetActive(true);

                    if (_cardDeck != null)
                        _cardDeck.AddCardIntoCardDeck(go.GetComponent<Card>().Data);
                }
                //... for different items
            }
            else
            {
                _item.gameObject.SetActive(false);
            }
        } 
    }
}