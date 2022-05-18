


namespace Tenacity.Cards.Data
{
    public enum RarityType
    {
        Common = 0,
        Rare = 1,
        Legendary = 2,
    }
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