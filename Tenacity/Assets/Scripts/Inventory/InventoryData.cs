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
        [SerializeField] private int _currency;
        [SerializeField] private int _maxSize;
        public List<CardData> _cards = new List<CardData>();

        public int Currency
        {
            get => _currency;
            set => _currency = value;
        }
        public List<CardData> Cards => _cards;


        public bool AddItem(Card item)
        {
            if (_cards.Count == _maxSize) return false;

            _cards.Add(item.Data);
            return true;
        }
    }
}
