using Tenacity.General.Items.Consumables;
using Tenacity.Cards.Inventory;
using Tenacity.General.Items;
using UnityEngine;
using System;


namespace Tenacity.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private EntityInventory _inventory;

        public int Currency => _inventory.Currency;
        

        private void GainCurrency(Coin coin, ConsumableTrigger consumeType)
        {
            _inventory.GainCurrency(coin.Count);
            coin.Consume(consumeType);
        }
        
        
        public void Consume(IItem item, ConsumableTrigger consumeType)
        {
            switch (item)
            {
                case Coin coin:
                    GainCurrency(coin, consumeType);
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}
