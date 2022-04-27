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
        private void Start()
        {
            MenuManager.Instance.OpenMenu(this);
        }


        public void OnStartAction()
        {
            MenuManager.Instance.CloseMenu(this);
            SceneManager.Instance.LoadMainGame(2, "Intro");
        }

        public void OnSettingsAction()
        {
            SettingsMenu.Show();
        }
        
        public void OnCredentialsAction()
        {
            AuthorsMenu.Show();
        }


        public override void OnBackAction()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();   
#endif
        }
    }
}