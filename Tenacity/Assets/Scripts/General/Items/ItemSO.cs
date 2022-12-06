using Tenacity.General.Items.Modes;
using UnityEngine;


namespace Tenacity.General.Items
{
    public abstract class ItemSO<T> : ScriptableObject, IDataItem
        where T : ItemSO<T>
    {
        [field: SerializeField] public bool UniqueStorageItem { get; private set; }
        [field: SerializeField] public ItemMode[] Modes { get; private set; }

        public abstract ItemRarity ItemRarity { get; }
        public abstract ItemType ItemType { get; }
        public abstract string Name { get; }
        public abstract int Id { get; }
    }
}