using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace Tenacity.Cards.Inventory
{
    public class InventoryMenu : MonoBehaviour // Make as SingleMenu
    {
        [SerializeField] private InventoryData data;
        [Space]
        [SerializeField] private GameObject[] cardSlots;
        [SerializeField] private Card inventoryItemPrefab;
        [SerializeField] private GameObject pageButtonPrefab;
        [SerializeField] private GameObject pageButtonsField;
        [SerializeField] private TextMeshProUGUI currentPageText;

        private List<CardDataSO> _cards = new List<CardDataSO>();
        private int _currentPage;
        private int _pageCount;

        
        private void Awake()
        {
            if ((data == null) || (inventoryItemPrefab == null) || (pageButtonsField == null) 
                || (pageButtonPrefab == null) || (currentPageText == null)) 
                return;

            _cards = data.Cards;
            _currentPage = 0;
            _pageCount = Mathf.CeilToInt(1.0f * _cards.Count / cardSlots.Length);

            DisplayItemsOnPage(_currentPage);
            CreatePageButtonList();
        }

        
        private void CreatePageButtonList()
        {
            if ((pageButtonsField == null) || (pageButtonPrefab == null)) return;

            for (int i = 0; i < _pageCount; i++)
                CreatePageButton(i);
        }
        
        private void CreateCard(int slotId)
        {
            var card = Instantiate(inventoryItemPrefab, cardSlots[slotId].transform);
            var cardTransform = card.transform;

            card.Data = _cards[slotId + _currentPage * cardSlots.Length];
            cardTransform.localScale = new Vector3(0.5f, 0.5f, 1);
            cardTransform.localPosition = Vector3.zero;
        }

        private void CreatePageButton(int pageNum)
        {
            var pageBtn = Instantiate(pageButtonPrefab, pageButtonsField.transform);
            var pageTransform = pageBtn.transform;
            
            pageTransform.localPosition = Vector3.zero;
            pageTransform.localScale = Vector3.one;
            
            pageBtn.GetComponent<Button>().onClick.AddListener(() => DisplayItemsOnPage(pageNum));
            pageBtn.GetComponentInChildren<TextMeshProUGUI>().text = (pageNum + 1).ToString();
        }
        
        
        public void DisplayItemsOnPage(int pageNum)
        {
            if ((_cards == null) || (_cards.Count == 0)) return;
            
            
            currentPageText.text = $"{pageNum + 1}/{_pageCount}";
            _currentPage = pageNum;

            for (int i = 0; i < cardSlots.Length; i++)
            {
                int cardId = i + _currentPage * cardSlots.Length;

                if (cardSlots[i].transform.childCount != 0)
                    Destroy(cardSlots[i].transform.GetChild(0).gameObject);
                if (cardId < _cards.Count)
                    CreateCard(i);
            }
        }
    }
}