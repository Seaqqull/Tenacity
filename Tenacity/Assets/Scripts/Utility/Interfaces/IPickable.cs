


namespace Tenacity.Utility.Interfaces
{
    public enum ItemType { None, Weapon, Ammo }
    
    public interface IPickable
    {
        public ItemType Type { get; }

        
        bool SetPickable(bool flag);
    }
}