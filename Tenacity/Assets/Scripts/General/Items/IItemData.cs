


namespace Tenacity.General.Items
{
    public enum ItemRarity
    {
        Common = 0,
        Rare = 1,
        Legendary = 2
    }
    
    public enum ItemType
    {
        Card, Story, Key, Currency
    }

    public interface IItem
    {
        ItemRarity ItemRarity { get; }
        ItemType ItemType { get; }
        string Name { get; }
        int Id { get; }
    }
}