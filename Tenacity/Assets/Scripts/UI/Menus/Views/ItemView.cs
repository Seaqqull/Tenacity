using Tenacity.General.Items;
using Tenacity.Base;


namespace Tenacity.UI.Menus.Views
{
    public abstract class ItemView : BaseMono
    {
        public abstract ItemType ViewType { get; }
        
        
        public abstract void ShowItemData(IItem item);


        public void SwitchView(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public virtual bool IsItemCompatible(IItem item)
        {
            return (item.ItemType & ViewType) != 0;
        }
    }
}