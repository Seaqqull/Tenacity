


namespace Tenacity.Cards.Data
{
    public enum CardState
    {
        InCardDeck,
        OnBoard,
        InHub,
        InInventory
    }
    
    public enum CardType
    {
        Creature = 0,
        Structure = 1,
        Event = 2,
        Hero = 4
    }
}