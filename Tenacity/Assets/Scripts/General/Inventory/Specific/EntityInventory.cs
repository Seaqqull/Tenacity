using Tenacity.General.Items.Consumables;
using System.Collections.Generic;
using Tenacity.General.Inventory;
using Tenacity.General.Items;
using UnityEngine;
using System.Linq;


namespace Tenacity.Cards.Inventory
{
    [CreateAssetMenu(fileName = "EntityInventory", menuName = "Inventory/Entity")]
    public class EntityInventory : ScriptableObject, IInventory<IItem>
    {
        [SerializeField] private int _currency;
        [Space] 
        [SerializeField] private CardsInventory _cardsInventory;
        [SerializeField] private StoriesInventory _storiesInventory;

        public IReadOnlyList<IItem> Items => _cardsInventory.Items.Concat<IItem>(_storiesInventory.Items).ToList().AsReadOnly();
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
        

        public void AddItem(IItem item)
        {
            
        }
        
        public bool AddStoryItem(StoryItem item)
        {
            return _storiesInventory.AddItem(item);
        }

        public void GetItem()
        {
            
        }

        public void RemoveItem(IItem item)
        {
            
        }
    }
}