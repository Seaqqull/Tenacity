using System.Collections.Generic;


namespace Tenacity.General.Inventory
{
    public interface IInventory<out TStore>
    {
        public IReadOnlyList<TStore> Items { get; }
    }
}