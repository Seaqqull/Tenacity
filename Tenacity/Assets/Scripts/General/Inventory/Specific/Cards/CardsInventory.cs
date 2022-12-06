using Tenacity.General.Inventory;
using UnityEngine;


namespace Tenacity.Cards.Inventory
{
    [CreateAssetMenu(fileName = "CardsInventory", menuName = "Inventory/Cards")]
    public class CardsInventory : Inventory<CardSO, CardItem>
    {
        public override bool AddItem(CardItem item)
        {
            return AddItem(item.Data);
        }

        public override bool AddItem(CardSO itemData)
        {
            if ((_items.Count == _size) || 
                (itemData.UniqueStorageItem && HasItem(itemData))) return false;

            _items.Add(itemData);
            return true;
        }
        
        public override bool RemoveItem(CardItem item)
        {
            return RemoveItem(item.Data);
        }
        
        public override bool RemoveItem(CardSO itemData)
        {
            if (!HasItem(itemData)) return false;

            _items.Remove(itemData);
            return true;
        }
    }
}
