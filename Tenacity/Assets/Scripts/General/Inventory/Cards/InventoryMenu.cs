using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;
using TMPro;


namespace Tenacity.Cards.Inventory
{
    public class InventoryMenu : MonoBehaviour // Make as SingleMenu
    {
        [SerializeField] private CardsInventory data;
        [SerializeField] private TMP_Text _pageText;
        [Space]
        [SerializeField] private GameObject[] cardSlots;
        [SerializeField] private Card inventoryItemPrefab;
        [SerializeField] private GameObject pageButtonPrefab;
        [SerializeField] private GameObject pageButtonsField;

        private List<(int Index, GameObject Object)> _buttons = new ();
        private List<CardSO> _cards = new List<CardSO>();
        private int _currentPage;
        private int _pageCount;

        
        private void Awake()
        {
            if ((data == null) || (inventoryItemPrefab == null) || (pageButtonsField == null) 
                || (pageButtonPrefab == null)) 
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

            List<(int Index, GameObject Object)> distinctButtons;
                
            _pageText.text = $"{_currentPage + 1}/{_pageCount}";
            foreach (var button in _buttons)
                Destroy(button.Object);
            _buttons.Clear();
            
            var addedButtons = 0;
            
            _buttons.Add((0, CreatePageButton(0, "1")));
            _buttons.Add(((_pageCount - 1), CreatePageButton((_pageCount - 1), $"{_pageCount}")));
            
            for (int i = -1; i < 2 && addedButtons < 3; i++)
            {
                var buttonPage = (_currentPage + i);
                if (buttonPage < 0 || buttonPage >= _pageCount)
                    continue;
                
                addedButtons++;
                _buttons.Add((buttonPage, CreatePageButton(
                    buttonPage,
                    (_currentPage + i + 1).ToString()
                )));
            }

            distinctButtons = _buttons.GroupBy(button => button.Index)
                .Select(button => button.First()).ToList();
            _buttons.ForEach(button => {
                if (!distinctButtons.Contains(button)) Destroy(button.Object);
            });
            _buttons = distinctButtons;
            
            if (_buttons.Count < 5)
            {
                var pageRelation = (float)_currentPage / _pageCount;
                var direction = (pageRelation >= 0.5f) ? -1 : 1;
                for (int i = (5 - _buttons.Count); i > 0; i--)
                {
                    var pageNumber = _currentPage + (direction * (1 + i));
                    _buttons.Add((pageNumber, CreatePageButton(
                        pageNumber,
                        (pageNumber + 1).ToString()
                    )));
                }
            }

            distinctButtons = _buttons.GroupBy(button => button.Index).
                Select(button => button.First()).
                Where(button => (button.Index < _pageCount) && (button.Index >= 0)).
                OrderBy(button => button.Index).ToList();
            _buttons.ForEach(button => {
                if (!distinctButtons.Contains(button)) Destroy(button.Object);
            });
            _buttons = distinctButtons;
            
            foreach (var button in _buttons)
                button.Object.transform.parent = pageButtonsField.transform;
        }
        
        private void CreateCard(int slotId)
        {
            var card = Instantiate(inventoryItemPrefab, cardSlots[slotId].transform);
            var cardTransform = card.transform;

            card.Data = _cards[slotId + _currentPage * cardSlots.Length];
            cardTransform.localPosition = Vector3.zero;
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