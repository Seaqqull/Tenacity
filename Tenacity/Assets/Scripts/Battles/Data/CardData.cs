using System.Collections.Generic;
using System.Linq;
using Tenacity.Cards;


namespace Tenacity.Battles.Data
{
    public interface ICardDeck
    {
        public IEnumerable<CardSO> Cards { get; }
        
        
        public int IndexOf(CardSO card);
    }

    public class PlayerCards : ICardDeck
    {
        public IEnumerable<CardSO> Cards { get; }

        
        public PlayerCards(IEnumerable<CardSO> cards)
        {
            Cards = cards.ToList();
        }

        public int IndexOf(CardSO card)
        {
            for (int i = 0; i < Cards.Count(); i++)
                if (Cards.ElementAt(i) == card)
                    return i;
            return -1;
        }
    }
}