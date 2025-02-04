using Tenacity.General.Items.Consumables;
using System.Collections.Generic;
using Tenacity.General.Inventory;
using Tenacity.General.Items;
using UnityEngine;
using System.Linq;


namespace Tenacity.Cards.Inventory
{
    [CreateAssetMenu(fileName = "EntityInventory", menuName = "Inventory/Entity")]
    public class EntityInventory : ScriptableObject, IInventory<IDataItem>
    {
        [SerializeField] private int _currency;
        [Space] 
        [SerializeField] private CardsInventory _cardsInventory;
        [SerializeField] private StoriesInventory _storiesInventory;

        public IReadOnlyList<IDataItem> Items => _cardsInventory.Items.Concat<IDataItem>(_storiesInventory.Items).ToList().AsReadOnly();
        public int Currency => _currency;
        
        
        public void GainCurrency(int amount)
        {
            _currency += amount;
        }

        public bool SpendCurrency(int amount)
        {
            if (!IsCurrencyEnough(amount))
                return false;

            _currency -= amount;
            return true;
        }

        public bool IsCurrencyEnough(int amount)
        {
            return (_currency >= amount);
        }
        

        public bool AddItem(IItem item)
        {
            switch (item.ItemType)
            {
                case ItemType.Card:
                    return _cardsInventory.AddItem(item as CardItem);
                case ItemType.Key:
                case ItemType.Story:
                    return _storiesInventory.AddItem(item as StoryItem);
                case ItemType.Currency:
                    GainCurrency((item as Coin).Count);
                    return true;
                default:
                    return false;
            }
        }

        public bool RemoveItem(IItem item)
        {
            switch (item.ItemType)
            {
                case ItemType.Card:
                    return _cardsInventory.RemoveItem(item as CardItem);
                case ItemType.Key:
                case ItemType.Story:
                    return _storiesInventory.RemoveItem(item as StoryItem);
                case ItemType.Currency:
                default:
                    return false;
            }
        }
        
        public bool AddItem(IDataItem item)
        {
            switch (item.ItemType)
            {
                case ItemType.Card:
                    return _cardsInventory.AddItem(item as CardSO);
                case ItemType.Key:
                case ItemType.Story:
                    return _storiesInventory.AddItem(item as StoryItemSO);
                case ItemType.Currency:
                    GainCurrency((item as CoinSO).Count);
                    return true;
                default:
                    return false;
            }
        }

        public bool RemoveItem(IDataItem item)
        {
            switch (item.ItemType)
            {
                case ItemType.Card:
                    return _cardsInventory.RemoveItem(item as CardSO);
                case ItemType.Key:
                case ItemType.Story:
                    return _storiesInventory.RemoveItem(item as StoryItemSO);
                case ItemType.Currency:
                default:
                    return false;
            }
        }

        public void InitializeInventory(int currency, IEnumerable<IDataItem> items)
        {
            _currency = currency;
            
            // Remove old
            var oldItems = Items.ToList();
            foreach (var oldItem in oldItems)
                RemoveItem(oldItem);
            
            // Add new
            foreach (var newItem in items)
                AddItem(newItem);
        }
    }
}