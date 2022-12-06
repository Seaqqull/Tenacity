using Button = UnityEngine.UI.Button;
using Tenacity.General.Items.Modes;
using Tenacity.Cards.Inventory;
using Tenacity.General.Items;
using UnityEngine;
using System.Linq;
using System;
using Tenacity.General.Items.Modifiers;
using TMPro;


namespace Tenacity.UI.Menus
{
    public class TradeMenu : SingleMenu<TradeMenu>
    {
        #region Constants
        private const string NO_MONEY_TEXT = "Not enough money";
        private const string NO_TRADE_TEXT = "Not for trade";
        private const int SILVER_CURRENCY_RATIO = 100;
        private const int GOLD_CURRENCY_RATIO = 10000;
        private const string SELL_TEXT = "Sell";
        private const string BUY_TEXT = "Buy";
        #endregion

        
        [field: SerializeField] private InventoryView SourceView { get; set; }
        [field: SerializeField] private InventoryView TargetView { get; set; }
        [field: Header("Selection")]
        [field: SerializeField] private GameObject CurrencyObject { get; set; }
        [field: SerializeField] private TMP_Text BronzeText { get; set; }
        [field: SerializeField] private TMP_Text SilverText { get; set; }
        [field: SerializeField] private TMP_Text GoldText { get; set; }
        [field: Space]
        [field: SerializeField] private Button ConfirmationButton { get; set; }
        [field: SerializeField] private TMP_Text ConfirmationText { get; set; }
        [field: Header("Data")]
        [field: SerializeField] public EntityInventory SourceInventory { get; private set; }
        [field: SerializeField] public EntityInventory TargetInventory { get; private set; }

        private IItemModifier _sourcePriceModifier;
        private IItemModifier _targetPriceModifier;
        
        private Action _onClose;
        
        public event Action OnClose
        {
            add { _onClose += value; }
            remove { _onClose -= value; }
        }


        private void HandleCurrency(int currency)
        {
            var gold = currency / GOLD_CURRENCY_RATIO;
            currency -= (gold * GOLD_CURRENCY_RATIO);
            var silver = currency / SILVER_CURRENCY_RATIO;
            currency -= (silver * SILVER_CURRENCY_RATIO);
            var bronze = currency % SILVER_CURRENCY_RATIO;
            
            GoldText.text = gold.ToString();
            SilverText.text = silver.ToString();
            BronzeText.text = bronze.ToString();
        }

        private void UpdateTradeInfo(int itemPrice, string allowTradeText)
        {
            var itemAllowable = (itemPrice >= 0) && (itemPrice <= SourceInventory.Currency);
            
            ConfirmationText.text = itemAllowable ? allowTradeText : (itemPrice < 0) ? NO_TRADE_TEXT : NO_MONEY_TEXT;
            ConfirmationButton.interactable = itemAllowable;
            ConfirmationButton.gameObject.SetActive(true);
            if (itemPrice >= 0)
            {
                CurrencyObject.SetActive(true);
                HandleCurrency(itemPrice);
            }
            else
            {
                CurrencyObject.SetActive(false);
            }
        }
        

        public void Initialize(EntityInventory sourceInventory, EntityInventory targetInventory, IItemModifier sourcePriceModifier, IItemModifier targetPriceModifier )
        {
            if ((sourceInventory == null) || (targetInventory == null))
            {
                Debug.LogError("[TradeMenu] Source and Target should not be null.");
                return;
            }

            _sourcePriceModifier = sourcePriceModifier ?? new DefaultItemModifier();
            _targetPriceModifier = targetPriceModifier ?? new DefaultItemModifier();
            SourceInventory = sourceInventory;
            TargetInventory = targetInventory;
            
            SourceView.UpdateView(SourceInventory);
            TargetView.UpdateView(TargetInventory);
        }


        public void OnSourceItemSelect(IDataItem item)
        {
            if (item == null) return;
            
            var itemPrice = item.Modes.
                Select(mode => mode as PriceItemMode).
                Where(mode => mode != null).
                Sum(priceMode => priceMode.Price);
            itemPrice = (int)(itemPrice * _targetPriceModifier.GetTradeValue(item.ItemType));
            
            UpdateTradeInfo(itemPrice, SELL_TEXT);

            ConfirmationButton.onClick.RemoveAllListeners();
            ConfirmationButton.onClick.AddListener(() =>
            {
                if (SourceInventory.RemoveItem(item))
                {
                    if (TargetInventory.AddItem(item))
                    {
                        SourceInventory.GainCurrency(itemPrice);
                        TargetInventory.SpendCurrency(itemPrice);    
                    }
                    else
                    {
                        SourceInventory.AddItem(item);
                    }
                }            
                
                TargetView.UpdateView(true);
                SourceView.UpdateView(true);
                CurrencyObject.SetActive(false);
                ConfirmationButton.gameObject.SetActive(false);
            });
        }

        public void OnTargetItemSelect(IDataItem item)
        {
            if (item == null) return;
            
            var itemPrice = item.Modes.
                Select(mode => mode as PriceItemMode).
                Where(mode => mode != null).
                Sum(priceMode => priceMode.Price);
            itemPrice = (int)(itemPrice * _sourcePriceModifier.GetTradeValue(item.ItemType));
            
            UpdateTradeInfo(itemPrice, BUY_TEXT);

            ConfirmationButton.onClick.RemoveAllListeners();
            ConfirmationButton.onClick.AddListener(() =>
            {
                if (TargetInventory.RemoveItem(item))
                {
                    if (SourceInventory.AddItem(item))
                    {
                        TargetInventory.GainCurrency(itemPrice);
                        SourceInventory.SpendCurrency(itemPrice);
                    }
                    else
                    {
                        TargetInventory.AddItem(item);
                    }
                }
                
                TargetView.UpdateView(true);
                SourceView.UpdateView(true);
                CurrencyObject.SetActive(false);
                ConfirmationButton.gameObject.SetActive(false);
            });
        }

        public override void OnBackAction()
        {
            base.OnBackAction();
            
            _onClose?.Invoke();
            _onClose = null;
        }
    }
}
