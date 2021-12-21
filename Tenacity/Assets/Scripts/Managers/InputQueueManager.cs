using Tenacity.Utility.Base;


namespace Tenacity.Managers
{
    public class InputQueueManager : SingleBehaviour<InputQueueManager>
    {
        private bool _inGameMenuActive;


        private void Start()
        {
            InputManager.BackButtonAction += OnShowInGameMenu;
        }


        private void OnShowInGameMenu(bool flag)
        {
            if (!flag)
                return;
            
            if (!_inGameMenuActive)
                UI.Menus.InGameMenu.Show();
            else
                UI.Menus.InGameMenu.Hide();
            _inGameMenuActive = !_inGameMenuActive;
        }


        public void OnHideInGameMenu()
        {
            _inGameMenuActive = false;
        }
    }
}