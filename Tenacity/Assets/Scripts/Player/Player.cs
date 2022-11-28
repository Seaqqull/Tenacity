using Tenacity.General.Items.Consumables;
using Tenacity.General.Inventory;
using Tenacity.Cards.Inventory;
using Tenacity.General.Items;
using UnityEngine;
using System;


namespace Tenacity.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private EntityInventory _inventory;

        public IInventory<IItem> Inventory => _inventory;
        public int Currency => _inventory.Currency;
        

        private void GainCurrency(Coin coin, ConsumableTrigger consumeTrigger)
        {
            _inventory.GainCurrency(coin.Count);
            coin.Consume(consumeTrigger);
        }

        private void AddStoryItems(StoryItem storyItem, ConsumableTrigger consumeTrigger)
        {
            if (_inventory.AddStoryItem(storyItem))
                storyItem.Consume(consumeTrigger);
        }


        public void Consume(IItem item, ConsumableTrigger consumeType)
        {
            switch (item)
            {
                case Coin coin:
                    GainCurrency(coin, consumeType);
                    break;
                case StoryItem book:
                    AddStoryItems(book, consumeType);
                    break;
                default:
                    throw new Exception("Undefined consume action");
            }
        }
    }
}
