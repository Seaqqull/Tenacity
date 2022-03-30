using System.Collections;
using System.Collections.Generic;
using Tenacity.Cards;
using Tenacity.Items;
using UnityEngine;

namespace Tenacity.PlayerInventory
{
    [CreateAssetMenu(fileName = "Inventory Template", menuName = "Inventory")]
    public class InventoryData : ScriptableObject
    {
        [SerializeField] private int currency;
        [SerializeField] private int maxSize;

        //replace by private
        public List<CardData> _cards = new List<CardData>();

        public int Currency
        {
            get => currency;
            set => currency = value;
        }
        public List<CardData> Cards => _cards;


        public bool AddItem(Card item)
        {
            if (_cards.Count == maxSize) return false;

            _cards.Add(item.Data);
            return true;
        }
    }
}
