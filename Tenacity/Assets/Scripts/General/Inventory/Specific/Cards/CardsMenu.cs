using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;
using TMPro;


namespace Tenacity.Cards.Inventory
{
    public class CardsMenu : MonoBehaviour // Make as SingleMenu
    {
        [SerializeField] private CardsInventory data;
        [SerializeField] private TMP_Text _pageText;
        [Space]
        [SerializeField] private GameObject[] cardSlots;
        [SerializeField] private CardItem inventoryItemPrefab;
        [SerializeField] private GameObject pageButtonPrefab;
        [SerializeField] private GameObject pageButtonsField;
        [SerializeField] private  int _pagesToShow = 5;

        private List<(int Index, GameObject Object)> _buttons = new ();
        private List<CardSO> _cards = new List<CardSO>();
        private int _currentPage;
        private int _pageCount;

        
        private void Awake()
        {
            if ((data == null) || (inventoryItemPrefab == null) || (pageButtonsField == null) 
                || (pageButtonPrefab == null)) 
                return;

            _cards = data.Items.ToList();
            _currentPage = 0;
            _pageCount = Mathf.CeilToInt(1.0f * _cards.Count / cardSlots.Length);

            DisplayItemsOnPage(_currentPage);
            CreatePageButtonList();
        }

        
        private void CreatePageButtonList()
        {
            if ((pageButtonsField == null) || (pageButtonPrefab == null)) return;
            var pages = GetPages(_currentPage, _pagesToShow, _pageCount);

                
            _pageText.text = $"{_currentPage + 1}/{_pageCount}";
            foreach (var button in _buttons)
                Destroy(button.Object);
            _buttons.Clear();

            foreach (var page in pages)
            {
                var button = CreatePageButton(page, (page + 1).ToString());
                button.transform.parent = pageButtonsField.transform;

                _buttons.Add((page, button));
            }
        }
        
        private void CreateCard(int slotId)
        {
            var card = Instantiate(inventoryItemPrefab, cardSlots[slotId].transform);
            var cardTransform = card.transform;

            card.Data = _cards[slotId + _currentPage * cardSlots.Length];
            cardTransform.localPosition = Vector3.zero;
        }
        
        private List<int> GetPages(int currentPage, int pagesPool, int pagesCount)
        {
            var distinctPages = new List<int>();
            
            // Min - Max pages
            distinctPages.Add(0);
            distinctPages.Add(pagesCount - 1);
            
            // Nearest pages
            var addedButtons = 0;
            for (int i = -1; i < 2 && addedButtons < 3; i++)
            {
                var buttonPage = (currentPage + i);
                if (buttonPage < 0 || buttonPage >= pagesCount)
                    continue;
                
                addedButtons++;
                distinctPages.Add(buttonPage);
            }
            // Store only distinct pages
            distinctPages = distinctPages.GroupBy(pageIndex => pageIndex)
                .Select(pageIndex => pageIndex.First()).ToList();
            
            
            if (distinctPages.Count < pagesPool)
            {
                var pageRelation = (float)currentPage / pagesCount;
                var direction = (pageRelation >= 0.5f) ? -1 : 1;
                for (int i = (pagesPool - distinctPages.Count); i > 0; i--)
                {
                    var pageNumber = currentPage + (direction * (1 + i));
                    distinctPages.Add(pageNumber);
                }
            }

            return distinctPages.GroupBy(pageIndex => pageIndex).
                Select(pageIndex => pageIndex.First()).
                Where(pageIndex => (pageIndex < pagesCount) && (pageIndex >= 0)).
                OrderBy(pageIndex => pageIndex).ToList();
        }

        private GameObject CreatePageButton(int pageNum, string text)
        {
            var pageBtn = Instantiate(pageButtonPrefab);// pageButtonsField.transform
            var pageTransform = pageBtn.transform;
            
            pageTransform.localPosition = Vector3.zero;
            pageTransform.localScale = Vector3.one;
            
            pageBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                DisplayItemsOnPage(pageNum);
                CreatePageButtonList();
            });
            pageBtn.GetComponentInChildren<TextMeshProUGUI>().text = text;
            return pageBtn;
        }
        
        
        public void DisplayItemsOnPage(int pageNum)
        {
            if ((_cards == null) || (_cards.Count == 0)) return;
            
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