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
        [SerializeField] private CardState state;

        private int _currentLife;

        public bool IsPlaced => gameObject.transform.parent.GetComponent<Land>();
        public int CurrentLife
        {
            get => _currentLife;
            set => _currentLife = value;
        }

        public bool IsAvailable
        {
            get => isAvailable;
            set => isAvailable = value;
        }
        public CardState State
        {
            get => state;
            set => state = value;
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

        
        public void GetDamaged(int power)
        {
            if (power == 0) return;

            _currentLife -= power;
            if (_currentLife <= 0)
            {
                _currentLife = 0;
                Destroy(gameObject, 1.0f);
            }
            if (TryGetComponent<CardDataDisplay>(out var cardDisplay)) 
                cardDisplay.UpdateLife();
        }
    }
}