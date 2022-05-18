using System.Collections.Generic;
using UnityEngine;


namespace Tenacity.Cards.Inventory
{
    [CreateAssetMenu(fileName = "Inventory Template", menuName = "Inventory")]
    public class InventoryData : ScriptableObject
    {
        [SerializeField] private int _currency;
        [SerializeField] private int _maxSize;
        [SerializeField] private List<CardDataSO> _cards;
        
        public List<CardDataSO> Cards => _cards;
        public int Currency
        {
            get => _currency;
            set => _currency = value;
        }

        
        public bool AddItem(Card item)
        {
            if (_cards.Count == _maxSize) return false;
            if (_cards.Contains(item.Data)) return false;

            _cards.Add(item.Data);
            return true;
        }
    }
}
