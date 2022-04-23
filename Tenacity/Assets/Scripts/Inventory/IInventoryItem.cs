using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tenacity.Items
{
    public interface IInventoryItem : IItem
    {
        bool IsStackable { get; }
        
    }
}