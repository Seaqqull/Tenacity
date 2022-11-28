using Tenacity.General.Inventory;
using UnityEngine;


namespace Tenacity.Cards.Inventory
{
    [CreateAssetMenu(fileName = "CardsInventory", menuName = "Inventory/Cards")]
    public class CardsInventory : Inventory<CardSO, Card>
    {
        public override bool AddItem(Card item)
        {
            if (_items.Count == _size) return false;
            if (HasItem(item.Data)) return false;

            _items.Add(item.Data);
            return true;
        }

        public override bool RemoveItem(Card item)
        {
            if (!HasItem(item.Data))
                return false;

            _items.Remove(item.Data);
            return true;
        }
    }
}
