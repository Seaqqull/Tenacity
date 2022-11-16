using UnityEngine;


namespace Tenacity.Cards.Inventory
{
    [CreateAssetMenu(fileName = "EntityInventory", menuName = "Inventory/Entity")]
    public class EntityInventory : ScriptableObject
    {
        [SerializeField] private int _currency;
        [Space] 
        [SerializeField] private CardsInventory _items;
        
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
        

        public void AddItem()
        {
            
        }

        public void GetItem()
        {
            
        }

        public void RemoveItem()
        {
            
        }
    }
}