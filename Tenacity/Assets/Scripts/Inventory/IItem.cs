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
        public ItemType ItemType { get; }
        public string Name { get; }
    }
}