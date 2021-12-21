using Tenacity.Managers;
using UnityEditor;


namespace Tenacity.UI.Menus
{
    public class InGameMenu : SingleMenu<InGameMenu>
    {
        public void OnExitAction()
        {
            InputQueueManager.Instance.OnHideInGameMenu();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();   
#endif
        }
        
        public void OnSettingsAction()
        {
            SettingsMenu.Show();
        }
        
        public void OnMainMenuAction()
        {
            MenuManager.Instance.CloseMenu(this);
            SceneManager.Instance.LoadMainMenu();
        }
        

        public override void OnBackAction()
        {
            MenuManager.Instance.CloseMenu(this);
            InputQueueManager.Instance.OnHideInGameMenu();
        }
    }
}