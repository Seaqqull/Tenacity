using Tenacity.General.Items.Consumables;
using System.Collections.Generic;
using Tenacity.General.Inventory;
using Tenacity.Cards.Inventory;
using Tenacity.General.Items;
using Tenacity.Cards;
using UnityEngine;
using System.Linq;
using System;


namespace Tenacity.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private EntityInventory _inventory;

        public IInventory<IDataItem> Inventory => _inventory;
        public int Currency => _inventory.Currency;
        

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
        
        public IDataItem RemoveItem(Func<IDataItem, bool> selector)
        {
            var itemToRemove = Inventory.Items.SingleOrDefault(selector);
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
        
        public IEnumerable<IDataItem> RemoveItems(Func<IDataItem, bool> selector)
        {
            var itemsToRemove = Inventory.Items.Where(selector).ToArray();
            foreach (var item in itemsToRemove)
            {
                _inventory.RemoveItem(item);
            }

            return itemsToRemove;
        }
    }
}
