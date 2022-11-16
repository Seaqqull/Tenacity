using System.Collections.Generic;
using UnityEngine;


namespace Tenacity.General.Inventory
{
    public abstract class Inventory<Tstore, Vdata> : ScriptableObject
    {
        [SerializeField] protected int _size;
        [SerializeField] protected List<Tstore> _items;

        public List<Tstore> Cards => _items;


        public abstract bool AddItem(Vdata item);

        public abstract bool RemoveItem(Vdata item);
    }
}