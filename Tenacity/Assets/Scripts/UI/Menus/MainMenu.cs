using Tenacity.Managers;
#if UNITY_EDITOR
using UnityEditor;
#else
using UnityEngine;   
#endif


namespace Tenacity.UI.Menus
{
    public class MainMenu : SingleMenu<MainMenu>
    {
        #region Constants
        private const int FIRST_LEVEL = 3;
        #endregion
        
        
        private void Start()
        {
            MenuManager.Instance.OpenMenu(this);
        }
        

        public void OnStartAction()
        {
            MenuManager.Instance.CloseMenu(this);
            SceneManager.Instance.LoadLevel(FIRST_LEVEL, "Swamps");
        }

        public void OnSaveLoadAction()
        {
            SaveLoadMenu.Show();
            SaveLoadMenu.Instance.SetMenuState(SaveLoadMenu.MenuState.Loadable);
        }

        public void OnSettingsAction()
        {
            SettingsMenu.Show();
        }
        
        public void OnQuitGameAction()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();   
#endif
        }
        
        public void OnCredentialsAction()
        {
            AuthorsMenu.Show();
        }
    }
}