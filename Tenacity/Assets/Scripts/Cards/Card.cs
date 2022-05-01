using Tenacity.Cards.Data;
using Tenacity.Base;
using UnityEngine;


namespace Tenacity.Cards
{
    public class Card : BaseMono
    {
        [SerializeField] private CardDataSO data;
        [SerializeField] private bool isAvailable;
        [SerializeField] private CardState state;

        private int _currentLife;
        
        public int CurrentLife => _currentLife;
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
        public CardDataSO Data
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