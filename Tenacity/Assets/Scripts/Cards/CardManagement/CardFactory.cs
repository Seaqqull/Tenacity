using System;
using UnityEngine;

namespace Tenacity.Cards
{
    [Serializable]
    public class CardFactory
    {
        [Flags]
        public enum CardType
        {
            None = 0x0,
            Standard = None,
            DefenseCard = 0x1,
            AttackCard = 0x2,
            CombinedCard = DefenseCard | AttackCard
        }
        public CardType Type = CardType.AttackCard;

        public CardTemplate CardTemplate = new CardTemplate();
        public AttackCard AttackCard = new AttackCard();
        public DefenseCard DefenseCard = new DefenseCard();
        public CombinedCard CombinedCard = new CombinedCard();

        public CardTemplate InitDefaultCard()
        {
            return GetCardByCardType(CardType.DefenseCard);
        }
        public Type GetClassByCardType(CardType cardType)
        {
            return GetCardByCardType(cardType).GetType();
        }
        public CardTemplate GetCurrentCardClass()
        {
            return GetCardByCardType(Type);
        }

        private CardTemplate GetCardByCardType(CardType cardType)
        {
            switch (cardType)
            {
                case CardType.AttackCard: return AttackCard;
                case CardType.DefenseCard: return DefenseCard;
                case CardType.CombinedCard: return CombinedCard;
                case CardType.Standard: return CardTemplate;
                default: return CombinedCard;
            }
        }
    }
}