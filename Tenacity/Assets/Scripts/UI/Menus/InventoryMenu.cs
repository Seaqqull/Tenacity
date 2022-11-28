using MainPlayer = Tenacity.Player.Player;
using System.Collections.Generic;
using Tenacity.General.Inventory;
using Tenacity.UI.Menus.Views;
using Tenacity.General.Items;
using Tenacity.UI.Additional;
using Tenacity.Managers;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;
using TMPro;


namespace Tenacity.UI.Menus
{
    public class InventoryMenu : SingleMenu<InventoryMenu>
    {
        #region Constants
        private const int SILVER_CURRENCY_RATIO = 100;
        private const int GOLD_CURRENCY_RATIO = 10000;
        #endregion
        
        [Space]
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private TMP_Text _silverText;
        [SerializeField] private TMP_Text _bronzeText;
        [SerializeField] private ItemView[] _views;
        [Space]
        [SerializeField] private TMP_Text _pageText;
        [Space]
        [SerializeField] private ClickableImage[] _itemSlots;
        [SerializeField] private GameObject _pageButtonPrefab;
        [SerializeField] private GameObject _pageButtonsField;
        [SerializeField] private  int _pagesToShow = 5;
        
        private List<IInventoryItem> ViewableItems
        {
            get
            {
                return _inventory.Items.Select(item => item as IInventoryItem).
                    Where(inventoryItem => inventoryItem != null).ToList();
            }
        }

        private List<(int Index, GameObject Object)> _buttons = new ();
        private IInventory<IItem> _inventory;
        private ItemType? _viewCategory;
        private int _currentPage;
        private int _pageCount;
        
        
        protected override void Awake()
        {
            base.Awake();

            var player = PlayerManager.Instance.GetComponent<MainPlayer>();
            var currency = player.Currency;
            
            _inventory = player.Inventory;
            
            HandleCurrency(currency);
            DisplayItemsOnPage(_currentPage);
            CreatePageButtonList();
        }


        private void UpdateView(IInventoryItem item)
        {
            foreach (var itemView in _views)
            {
                var viewCompatible = (item != null) && itemView.IsItemCompatible(item);
                itemView.SwitchView(viewCompatible);
                            
                if (viewCompatible)
                    itemView.ShowItemData(item);
            }
        }

        private void DisplayItemsOnPage(int page)
        {
            var itemsToView = ViewableItemsWithCategory();
            
            _pageCount = Mathf.CeilToInt((float)itemsToView.Count / _itemSlots.Length);
            _currentPage = page;
            
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                int itemIndex = (_currentPage * _itemSlots.Length) + i;
                
                if (itemIndex < itemsToView.Count())
                {
                    var item = itemsToView.ElementAt(itemIndex);
                    _itemSlots[i].AssignAction(item.InventoryView, () =>
                    {
                        UpdateView(item);
                    });
                }
                else
                {
                    _itemSlots[i].AssignAction(null, null);
                }
            }
        }
        
        private void HandleCurrency(int currency)
        {
            var gold = currency / GOLD_CURRENCY_RATIO;
            currency -= (gold * GOLD_CURRENCY_RATIO);
            var silver = currency / SILVER_CURRENCY_RATIO;
            currency -= (silver * SILVER_CURRENCY_RATIO);
            var bronze = currency % SILVER_CURRENCY_RATIO;
            
            _goldText.text = gold.ToString();
            _silverText.text = silver.ToString();
            _bronzeText.text = bronze.ToString();
        }
        
        private GameObject CreatePageButton(int pageNum, string text)
        {
            var pageBtn = Instantiate(_pageButtonPrefab, _pageButtonsField.transform);
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
        
        private List<IInventoryItem> ViewableItemsWithCategory()
        {
            return (!_viewCategory.HasValue) ? ViewableItems : 
                ViewableItems.Where(inventoryItem => inventoryItem.ItemType == _viewCategory).ToList();
        }
        
#region Pages
        private void CreatePageButtonList()
        {
            if ((_pageButtonsField == null) || (_pageButtonPrefab == null)) return;
            var pages = GetPages(_currentPage, _pagesToShow, _pageCount);

                
            _pageText.text = $"{_currentPage + 1}/{_pageCount}";
            foreach (var button in _buttons)
                Destroy(button.Object);
            _buttons.Clear();

            foreach (var page in pages)
            {
                var button = CreatePageButton(page, (page + 1).ToString());
                button.transform.parent = _pageButtonsField.transform;

                _buttons.Add((page, button));
            }
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
#endregion


        public void SetFiltering(int filterIndex)
        {
            _viewCategory = (Enum.IsDefined(typeof(ItemType), filterIndex)) ? (ItemType) filterIndex : null;
            _currentPage = 0;

            
            UpdateView(null);
            
            DisplayItemsOnPage(_currentPage);
            CreatePageButtonList();
        }
    }
}
