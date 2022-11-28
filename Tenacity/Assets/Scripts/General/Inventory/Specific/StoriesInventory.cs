using Tenacity.General.Items.Consumables;
using UnityEngine;


namespace Tenacity.General.Inventory
{
    [CreateAssetMenu(fileName = "StoriesInventory", menuName = "Inventory/Stories")]
    public class StoriesInventory : Inventory<StoryItemSO, StoryItem>
    {
        public override bool AddItem(StoryItem item)
        {
            if ((_items.Count == _size) || 
                (item.Data.UniqueStorageItem && HasItem(item.Data))) return false;

            _items.Add(item.Data);
            return true;
        }

        public override bool RemoveItem(StoryItem item)
        {
            if (!HasItem(item.Data)) return false;

            _items.Remove(item.Data);
            return true;
        }
    }
}