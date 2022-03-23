using System.Collections;
using System.Collections.Generic;
using Tenacity.Cards;
using Tenacity.Items;
using UnityEngine;

namespace Tenacity.PlayerInventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private InventoryData data;

        public InventoryData Data => data;

        public bool AddItem(Card item)
        {
            return data.AddItem(item);
        }
    }
}
