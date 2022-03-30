using Tenacity.Items;
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
        [SerializeField] private bool isDraggable;

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
        public bool IsDraggable
        {
            get => isDraggable;
            set => isDraggable = value;
        }

    }
}