using Tenacity.General.Items.Modes;
using UnityEngine;


namespace Tenacity.General.Items
{
    public abstract class Item<T> : MonoBehaviour, IItem
        where T : ItemSO<T>
    {
        [SerializeField] private T _data;
        [Space] 
        [SerializeField] private bool _overrideName;
        [SerializeField] private string _itemName;

        public ItemRarity ItemRarity => _data.ItemRarity;
        public ItemType ItemType => _data.ItemType;
        public ItemMode[] Modes => _data.Modes;
        public string Name => (_overrideName) ? _itemName : _data.Name;
        public int Id => _data.Id;
        public T Data => _data;
    }
}