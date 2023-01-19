using Tenacity.Cards.Data;
using Tenacity.Base;
using UnityEngine;
using Tenacity.Battles.Lands;
using Tenacity.General.Items;
using Tenacity.General.Items.Consumables;

namespace Tenacity.Cards
{
    public class CardItem : EnvironmentItem<CardSO, CardItem>, IConsumable
    {
        public ConsumableTrigger Trigger => ConsumableTrigger.Pickup;
        public int ReusableCount => int.MaxValue;

        
        public void Consume(ConsumableTrigger trigger)
        {
            Destroy(gameObject);
        }
        [SerializeField] private CardSO data;
        [SerializeField] private bool isAvailable;

        private int _currentLife;

        public int CurrentLife
        {
            get => _currentLife;
            set => _currentLife = value;
        }
        public CardSO Data
        {
            get => data;
            set => data = value;
        }


        private void Start()
        {
            if (Data != null)
                _currentLife = Data.Life;
        }
    }
}