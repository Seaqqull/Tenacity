


namespace Tenacity.Cards.Inventory
{
    public enum ItemType
    {
        Card,
        Key
    }

    public interface IItem
    {
        ItemType ItemType { get; }
        string Name { get; }
    }
}