using System.Collections;
using System.Collections.Generic;
using Tenacity.Cards;
using Tenacity.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tenacity.PlayerInventory
{
    public class InventorySlotsController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject detailsViewScreen;

        private Card _item;

        private void Start()
        {
            if (detailsViewScreen == null)
                return;
            _item = detailsViewScreen.GetComponentInChildren<Card>();
            _item.gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GameObject go = eventData.pointerCurrentRaycast.module.gameObject;

            if (go != null)
            {
                if (go.GetComponent<Card>())
                {
                    _item.GetComponent<Card>().Data = go.GetComponent<Card>().Data;
                    _item.GetComponent<CardDataDisplayUI>().DisplayCardValues();
                    _item.gameObject.SetActive(true);
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