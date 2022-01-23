using UnityEngine;


namespace Tenacity.UI
{
    public abstract class Menu : Base.BaseMono
    {
        [SerializeField] protected bool _destroyOnClose = true;
        [SerializeField] protected bool _disableBelowMenus = true;

        public bool DisableBelowMenus
        {
            get { return _disableBelowMenus; }
        }
        public bool DestroyOnClose
        {
            get { return _destroyOnClose; }
        }


        public abstract void OnBackAction();
    }
}