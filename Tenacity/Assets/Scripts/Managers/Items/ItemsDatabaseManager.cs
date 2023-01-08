using Tenacity.General.Items.Consumables;
using System.Collections.Generic;
using Tenacity.General.Items;
using Tenacity.Cards;
using Tenacity.Base;
using System.Linq;
using UnityEngine;


namespace Tenacity.Managers
{
    public class ItemsDatabaseManager : SingleBehaviour<ItemsDatabaseManager>
    {
        [SerializeField] private List<ItemSO<StoryItemSO>> _stories;
        [SerializeField] private List<ItemSO<CardSO>> _cards;
        
        public IReadOnlyList<IDataItem> Items => _cards.Concat<IDataItem>(_stories).ToList().AsReadOnly();


        public IEnumerable<IDataItem> GetItems(IEnumerable<int> itemIds)
        {
            return itemIds.Select(itemId => Items.Single(item => item.Id == itemId)).ToList();
        }
    }
}