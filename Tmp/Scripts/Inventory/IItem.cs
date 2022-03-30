using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tenacity.Items { 

    public enum ItemType
    {
        Card,
        Key
    }

    public interface IItem
    {
        ItemType ItemType { get; }
        string Name { get; }
    }
}