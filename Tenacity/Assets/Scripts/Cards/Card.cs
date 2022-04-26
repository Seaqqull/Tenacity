using Tenacity.Items;
using Tenacity.Lands;
using UnityEngine;

namespace Tenacity.Cards
{
    public enum CardState
    {
        InCardDeck,
        OnBoard,
        InHub,
        InInventory
    }

    public class Card : MonoBehaviour
    {
        [SerializeField] private CardData data;
        [SerializeField] private bool isAvailable;
        [SerializeField] private CardState state;

        public CardData Data
        {
            get => data;
            set => data = value;
        }
        public CardState State
        {
            get => state;
            set => state = value;
        }
        public bool IsAvailable
        {
            get => isAvailable;
            set => isAvailable = value;
        }
        public int CurrentLife => _currentLife;
        public Land Place {
            get => transform.parent?.GetComponent<Land>();
            set => transform.SetParent(value.transform);
        }

        private int _currentLife;


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
            GetComponent<CardDataDisplay>()?.UpdateLife();
        }
    }
}