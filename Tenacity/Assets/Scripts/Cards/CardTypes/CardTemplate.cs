using System;
using UnityEngine;

namespace Tenacity.Cards
{
    [Serializable]
    public class CardTemplate
    {
        public enum RarityTier
        {
            None = 0,
            Common = 1,
            Rare = 2,
            Epic = 4,
            Legendary = 8
        }

        [SerializeField]
        private int cardID;
        [SerializeField]
        private string cardName;
        [SerializeField, Multiline]
        private string cardDescription;
        [SerializeField]
        private int cardCost;
        [SerializeField]
        private RarityTier rarityTier;
        [SerializeField]
        private Sprite creature;

        public int CardID { get => cardID; }
        public int CardCost { get => cardCost; }
        public string CardName { get => cardName; }
        public Sprite Creature { get => creature; }
        public string CardDescription { get => cardDescription; }
    }
}