using Tenacity.Managers;
using Tenacity.UI.Menus;
using UnityEngine;  
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Tenacity.UI
{
    public class UserActionsMenu : SingleMenu<UserActionsMenu>
    {
        public void OnSettingsAction()
        {
            SettingsMenu.Show();
        }

        public void OnInventoryAction()
        {
            InventoryMenu.Show();
        }

        public void OnInGameMenuAction()
        {
            InGameMenu.Show();
        }

        public override void OnBackAction()
        {
            MenuManager.Instance.CloseMenu(this);
            InputQueueManager.Instance.OnHideInGameMenu();
        }
    }
}
