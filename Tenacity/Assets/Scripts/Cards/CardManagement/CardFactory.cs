using System;
using UnityEngine;

[Serializable]
public class CardFactory
{
    [Flags]
    public enum CardType
    {
        None,
        DefenseCard,
        AttackCard,
        CombinedCard = AttackCard | DefenseCard
    }
    public CardType Type = CardType.None;

    public Card Card = new Card();
    public AttackCard AttackCard = new AttackCard();
    public DefenseCard DefenseCard = new DefenseCard();
    public CombinedCard CombinedCard = new CombinedCard();

    public Card InitDefaultCard()
    {
        return GetCardByCardType(CardType.DefenseCard);
    }
    public Type GetClassByCardType(CardType cardType)
    {
        return (GetCardByCardType(cardType)).GetType();
    }

    private Card GetCardByCardType(CardType cardType)
    {
        switch(cardType)
        {
            case CardType.None: return Card;
            case CardType.AttackCard: return AttackCard;
            case CardType.DefenseCard: return DefenseCard;
            case CardType.CombinedCard: return CombinedCard;
            default: return CombinedCard;
        }
    }
}
