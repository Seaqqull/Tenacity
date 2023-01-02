using System;
using Tenacity.General.Items.Modes;
using UnityEngine;


namespace Tenacity.General.Items
{
    public enum ItemRarity
    {
        Common = 0,
        Rare = 1,
        Legendary = 2
    }

    [Flags]
    public enum ItemType
    {
        /*None = -1, */
        Card,
        Story,
        Key,
        Currency
    }

    public interface IItem
    {
        ItemRarity ItemRarity { get; }
        ItemType ItemType { get; }
        ItemMode[] Modes { get; }
        string Name { get; }
        int Id { get; }
    }

    public interface IDataItem : IItem
    {
        public bool UniqueStorageItem { get; }
    }
    

    public interface IInventoryItem : IDataItem
    {
        Sprite InventoryView { get; }
    }
}