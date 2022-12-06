using Tenacity.General.Items.Consumables;
using UnityEngine;


namespace Tenacity.General.Inventory
{
    [CreateAssetMenu(fileName = "StoriesInventory", menuName = "Inventory/Stories")]
    public class StoriesInventory : Inventory<StoryItemSO, StoryItem>
    {
        public override bool AddItem(StoryItem item)
        {
            return AddItem(item.Data);
        }

        public override bool RemoveItem(StoryItem item)
        {
            return RemoveItem(item.Data);
        }
        
        public override bool AddItem(StoryItemSO itemData)
        {
            if ((_items.Count == _size) || 
                (itemData.UniqueStorageItem && HasItem(itemData))) return false;

            _items.Add(itemData);
            return true;
        }

        public override bool RemoveItem(StoryItemSO itemData)
        {
            if (!HasItem(itemData)) return false;

            _items.Remove(itemData);
            return true;
        }
    }
}