using System.Collections.Generic;
using UnityEngine;


namespace Tenacity.General.Inventory
{
    public abstract class Inventory<Tstore, Vdata> : ScriptableObject, IInventory<Tstore>
    {
        [SerializeField] protected int _size;
        [SerializeField] protected List<Tstore> _items;

        public IReadOnlyList<Tstore> Items => _items.AsReadOnly();


        public bool HasItem(Tstore data)
        {
            return _items.Contains(data);
        }

        public abstract bool AddItem(Vdata item);
        public abstract bool AddItem(Tstore item);
        public abstract bool RemoveItem(Vdata item);
        public abstract bool RemoveItem(Tstore item);
    }
}