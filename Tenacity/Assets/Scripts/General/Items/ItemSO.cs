using UnityEngine;


namespace Tenacity.General.Items
{
    public abstract class ItemSO<T> : ScriptableObject, IItem 
        where T : ItemSO<T>
    {
        public abstract ItemRarity ItemRarity { get; }
        public abstract ItemType ItemType { get; }
        public abstract string Name { get; }
        public abstract int Id { get; }
    }
}