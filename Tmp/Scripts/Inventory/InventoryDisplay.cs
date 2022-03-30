using System.Collections;
using System.Collections.Generic;
using Tenacity.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tenacity.PlayerInventory
{
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject[] cardSlots;
        [SerializeField] private Card inventoryItemPrefab;
        [SerializeField] private GameObject pageButtonPrefab;
        [SerializeField] private GameObject pageButtonsField;
        [SerializeField] private TextMeshProUGUI currentPageText;

        private int _currentPage;

        private int _pageCount;
        private Inventory _inventory;
        private List<CardData> _cards = new List<CardData>();

        private void Awake()
        {
            if (!transform.GetComponent<Inventory>()
                || inventoryItemPrefab == null
                || pageButtonsField == null
                || pageButtonPrefab == null
                || currentPageText == null) 
                return;

            _inventory = GetComponent<Inventory>();
            _cards = _inventory.Data.Cards;
            _currentPage = 0;
            _pageCount = Mathf.CeilToInt(1.0f * _cards.Count / cardSlots.Length);

            DisplayItemsOnPage(_currentPage);
            CreatePageButtonList();
        }

        public void DisplayItemsOnPage(int pageNum)
        {
            if (_cards == null || _cards.Count == 0) return;
            _currentPage = pageNum;
            
            currentPageText.text = $"{pageNum + 1}/{_pageCount}";

            for (int i = 0; i < cardSlots.Length; i++)
            {
                int cardId = i + _currentPage * cardSlots.Length;

                if (cardSlots[i].transform.childCount != 0)
                    Destroy(cardSlots[i].transform.GetChild(0).gameObject);
                if (cardId < _cards.Count)
                    CreateCard(i);
            }
        }

        private void CreatePageButtonList()
        {
            if (pageButtonsField == null || pageButtonPrefab == null) return;

            for (int i = 0; i < _pageCount; i++)
            {
                CreatePageButton(i);
            }
        }

        private void CreatePageButton(int pageNum)
        {
            GameObject pageBtn = Instantiate(pageButtonPrefab) as GameObject;
            pageBtn.transform.SetParent(pageButtonsField.transform);
            pageBtn.transform.localPosition = Vector3.zero;
            pageBtn.transform.localScale = Vector3.one;
            pageBtn.GetComponentInChildren<TextMeshProUGUI>().text = (pageNum + 1).ToString();
            pageBtn.GetComponent<Button>().onClick.AddListener(() => DisplayItemsOnPage(pageNum));
        }

        private void CreateCard(int slotId)
        {
            var card = Instantiate(inventoryItemPrefab);
            card.transform.SetParent(cardSlots[slotId].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            card.Data = _cards[slotId + _currentPage * cardSlots.Length];
        }

    }
}