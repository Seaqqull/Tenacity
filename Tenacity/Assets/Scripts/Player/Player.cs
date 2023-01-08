using Tenacity.General.Items.Consumables;
using Tenacity.General.SaveLoad.Data;
using System.Collections.Generic;
using Tenacity.General.Inventory;
using Tenacity.General.SaveLoad;
using Tenacity.Cards.Inventory;
using Tenacity.General.Items;
using Tenacity.Managers;
using Tenacity.Cards;
using UnityEngine;
using System.Linq;
using System;


namespace Tenacity.Player
{
    [System.Serializable]
    public class PlayerSnap : SaveSnap
    {
        public IEnumerable<int> ItemIds { get; private set; }
        public int Currency { get; private set; }


        public PlayerSnap(MonoBehaviour behaviour, int currency, IEnumerable<int> items) : base(behaviour)
        {
            ItemIds = items.ToList();
            Currency = currency;
        }
        public PlayerSnap(string id, int currency, IEnumerable<int> items) : base(id)
        {
            ItemIds = items.ToList();
            Currency = currency;
        }
    }
    
    public class Player : MonoBehaviour, ISavable
    {
        #region Constants
        private const string SAVE_ID = "Player"; 
        #endregion
        
        [SerializeField] private EntityInventory _defaultInventory;
        [SerializeField] private EntityInventory _inventory;

        public IInventory<IDataItem> Inventory => _inventory;
        public int Currency => _inventory.Currency;
        public string Id => SAVE_ID;
        

        private void Start()
        {
            SaveLoadManager.Instance.AddToSavable(this);
        }

        private void OnDestroy()
        {
            SaveLoadManager.Instance.RemoveFromSavable(this);
        }
        

        private void GainCurrency(Coin coin, ConsumableTrigger consumeTrigger)
        {
            _inventory.GainCurrency(coin.Count);
            coin.Consume(consumeTrigger);
        }

        private void AddItem(CardItem cardItem, ConsumableTrigger consumeTrigger)
        {
            if (_inventory.AddItem(cardItem.Data))
                cardItem.Consume(consumeTrigger);
        }
        
        private void AddItem(StoryItem storyItem, ConsumableTrigger consumeTrigger)
        {
            if (_inventory.AddItem(storyItem.Data))
                storyItem.Consume(consumeTrigger);
        }


        public bool HasItem(int id)
        {
            return Inventory.Items.Any(item => item.Id == id);
        }
        
        public void ResetInventory()
        {
            _inventory.InitializeInventory(_defaultInventory.Currency, _defaultInventory.Items);
        }
        
        public void Consume(IDataItem item)
        {
            switch (item)
            {
                case CoinSO coin:
                    _inventory.AddItem(coin);
                    break;
                case CardSO card:
                    _inventory.AddItem(card);
                    break;
                case StoryItemSO story:
                    _inventory.AddItem(story);
                    break;
                default:
                    throw new Exception("Undefined consume action");
            }
        }
        
        public IDataItem RemoveItem(Func<IDataItem, bool> selector)
        {
            var itemToRemove = Inventory.Items.FirstOrDefault(selector);
            _inventory.RemoveItem(itemToRemove);

            return itemToRemove;
        }
        
        public void Consume(IItem item, ConsumableTrigger consumeType)
        {
            switch (item)
            {
                case Coin coin:
                    GainCurrency(coin, consumeType);
                    break;
                case CardItem card:
                    AddItem(card, consumeType);
                    break;
                case StoryItem story:
                    AddItem(story, consumeType);
                    break;
                default:
                    throw new Exception("Undefined consume action");
            }
        }
        
        public IEnumerable<IDataItem> RemoveItems(Func<IDataItem, bool> selector)
        {
            var itemsToRemove = Inventory.Items.Where(selector).ToArray();
            foreach (var item in itemsToRemove)
            {
                _inventory.RemoveItem(item);
            }

            return itemsToRemove;
        }

        #region Savable
        public SaveSnap MakeSnap()
        {
            return new PlayerSnap(Id, _inventory.Currency, _inventory.Items.Select(item => item.Id).ToList());
        }

        public void FromSnap(SaveSnap data)
        {
            var playerSnap = data as PlayerSnap;
            if (playerSnap == null) return;

            _inventory.InitializeInventory(playerSnap.Currency, ItemsDatabaseManager.Instance.GetItems(playerSnap.ItemIds));
        }
        #endregion
    }
}
